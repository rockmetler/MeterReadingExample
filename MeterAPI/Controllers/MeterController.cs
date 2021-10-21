
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MeterAPI.BL.Interface;

using MeterContracts;

using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using MeterAPI.Helpers.Interfaces;
using System.Linq;

namespace MeterAPI.Controllers
{
    [ApiController]
    public class MeterController : ControllerBase
    {
        private readonly IMeterService _MeterService;
        public MeterController(IMeterService MeterService)
        {
            _MeterService = MeterService;
        }

        /// <summary>
        /// Uploads a CSV of meter readings to the server which then validates and returns a read out of successful and unsuccessful results.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("meter-reading-uploads")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadMeterReadings(IFormFile file)
        {
            bool filecheck = false;
            switch (file.ContentType)
            {
                case "application/csv":
                case "text/csv":
                case "application/vnd.ms-excel":
                    filecheck = true;
                    break;
            }
            if (file.Length > 0 && filecheck)
            {
                var request = await _MeterService.ProcessMeterCSV(file);
                if (request.MeterRequest.Count > 0)
                {
                    var response = await _MeterService.CreateMeterReadings(request);
                    int successful = response.Where(x => x.Success).Count();
                    int failed = response.Where(x => !x.Success).Count();
                    return Ok(new { SuccessfulImports = successful, FailedImports = failed });
                }
            }
            else
            {
                return BadRequest("file is not valid.");
            }
            return BadRequest("Something went wrong while processing your request. Please validate your file, and try again");
        }

        [HttpDelete]
        [Route("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteMeterReading(string MeterReadingId)
        {
            var response = await _MeterService.DeleteMeterReading(MeterReadingId);
            if (response)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest("Delete request did not return successful.");
            }
        }

        [HttpGet]
        [Route("Get/{MeterReadingId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetMeterReading([FromRoute] string MeterReadingId)
        {
            var response = _MeterService.GetMeterReading(MeterReadingId);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest("Get request did not return successful.");
            }
        }

        [HttpPost]
        [Route("Upsert")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpsertMeterReading(MeterRequest request)
        {
            var response = await _MeterService.UpsertMeterReading(request);
            if (response != null)
            {
                if (response.Success)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest("Upsert request did not return successful.");
                }
            }
            else
            {
                return BadRequest("request did not return successful, please ensure your meter reading is associated with a valid account ID");
            }
        }

    }
}
