using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SMSGateway.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMSGateway.Server.Models;
using SMSGateway.Shared;

namespace SMSGateway.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;

        public HistoryController(IHistoryService historyService)
        {
            _historyService = historyService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllFiltered(string referenceId, string createdByUserId)
        {
            return Ok(_historyService.GetAllFiltered(referenceId, createdByUserId));
        }

        [HttpPost()]
        public async Task<IActionResult> CreateAsync([FromBody]History model)
        {
            if (ModelState.IsValid)
            {
                var result = await _historyService.CreateAsync(model);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest(
            new OperationResponse<ContactGroup>
            {
                IsSuccess = false,
                Message = "Internal server error"
            }); //400
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateAsync(string referenceId, string status)
        {
            var result = await _historyService.UpdateAsync(referenceId, status);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
