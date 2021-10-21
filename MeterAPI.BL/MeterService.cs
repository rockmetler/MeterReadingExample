using MeterAPI.BL.Interface;
using MeterAPI.DAL.Entities;
using MeterAPI.DAL.Interface;
using MeterAPI.Helpers.Interfaces;

using MeterContracts;

using Microsoft.AspNetCore.Http;

using MoreLinq;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MeterAPI.BL
{
    public class MeterService : IMeterService
    {
        private readonly IMeter _Meter;
        private readonly IHelpers _helpers;
        public MeterService(IMeter Meter, IHelpers helpers)
        {
            _Meter = Meter;
            _helpers = helpers;
        }

        public async Task<List<MeterResponse>> CreateMeterReadings(MeterRequestContainer request)
        {
            List<MeterReadingEntity> entityList = request.MeterRequest.Select(x => (MeterReadingEntity)x).ToList();
            List<MeterReadingEntity> invalidList = request.MeterRequest.Select(x => (MeterReadingEntity)x).ToList();
            EntityContainer container = new EntityContainer()
            {
                InvalidList = invalidList,
                ValidList = entityList
            };
            var result = await _Meter.CreateMeterReadings(container);
            return result;
        }

        public async Task<MeterRequestContainer> ProcessMeterCSV(IFormFile file)
        {
            MeterRequestContainer returnValue = new MeterRequestContainer()
            {
                MeterRequest = new List<MeterRequest>(),
                InvalidValues = new List<MeterRequest>()
            };
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                //go to position 0 so we dont hit EOF
                stream.Position = 0;
                TextReader textReader = new StreamReader(stream);
                var lines = _helpers.ReadAllLines(textReader);
                returnValue.MeterRequest = lines.Skip(1).Select(x => MeterRequest.FromCsv(x)).ToList();
            }
            if (returnValue.MeterRequest.Count > 0)
            {
                returnValue.InvalidValues = returnValue.MeterRequest.GroupBy(x => x.AccountId).Where(x => x.Count() > 1).SelectMany(x => x).ToList();
                returnValue.MeterRequest = returnValue.MeterRequest.DistinctBy(x => x.AccountId).ToList();
            }
            return returnValue;
        }
        /// <summary>
        /// Deletes a MeterReading from the database
        /// </summary>
        /// <param name="MeterReadingId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteMeterReading(string MeterReadingId)
        {
            var response = await _Meter.DeleteMeterReading(MeterReadingId);
            return response;
        }


        /// <summary>
        /// Gets a MeterReading from the database
        /// </summary>
        /// <param name="MeterReadingId"></param>
        /// <returns></returns>
        public MeterResponse GetMeterReading(string MeterReadingId)
        {
            var response = _Meter.GetByMeterReadingId(MeterReadingId);
            return response;
        }

        /// <summary>
        /// Updates the data stored related to a MeterReading
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<MeterResponse> UpsertMeterReading(MeterRequest request)
        {
            var Entity = new MeterReadingEntity()
            {
                AccountId = request.AccountId,
                MeterReadingId = request.MeterReadingId,
                Reading = request.Reading
            };
            var response = await _Meter.UpsertMeterReading(Entity);
            return response;
        }


    }
}
