using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMSGateway.Server.Models;
using SMSGateway.Server.Services;
using SMSGateway.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet("GetLog")]
        public async Task<IActionResult> GetAllFiltered(string from, string sendTo, string messages, DateTime timeStart, DateTime timeEnd)
        {
            return Ok(_logService.GetAllFiltered(from, sendTo, messages, timeStart, timeEnd));
        }

        [HttpPost("CreateLog")]
        public async Task<IActionResult> CreateAsync([FromBody]Log model)
        {
            if (ModelState.IsValid)
            {
                var result = await _logService.CreateAsync(model);

                if (result.IsSuccess)
                {
                    return Ok(result); //200
                }

                return BadRequest(result);
            }

            return BadRequest(
            new OperationResponse<Role>
            {
                IsSuccess = false,
                Message = "Internal server error"
            }); //400
        }
    }
}
