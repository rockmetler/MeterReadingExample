using AccountContracts;

using MeterAPI.BL.Interface;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeterAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpDelete]
        [Route("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAccount(string AccountId)
        {
            var response = await _accountService.DeleteAccount(AccountId);
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
        [Route("Get/{AccountId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetAccount([FromRoute] string AccountId)
        {
            var response = _accountService.GetAccount(AccountId);
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
        public async Task<IActionResult> UpsertAccount(AccountRequest request)
        {
            var response = await _accountService.UpsertAccount(request);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest("Upsert request did not return successful.");
            }
        }

    }
}
