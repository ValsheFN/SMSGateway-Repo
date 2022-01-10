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
    public class TopUpController : ControllerBase
    {
        private readonly ITopUpService _topUpService;

        public TopUpController(ITopUpService topUpService)
        {
            _topUpService = topUpService;
        }

        [HttpGet("GetTopUp")]
        public async Task<IActionResult> GetAllFiltered(string referenceId, string requester, string status,
                                          DateTime requestDateStart, DateTime requestDateEnd,
                                          DateTime grantDateStart, DateTime grantDateEnd,
                                          string grantedBy)
        {
            return Ok(_topUpService.GetAllFiltered(referenceId, requester, status,
                                          requestDateStart, requestDateEnd,
                                          grantDateStart, grantDateEnd,
                                          grantedBy));
        }

        [HttpPost("CreateTopUp")]
        public async Task<IActionResult> CreateAsync([FromBody] TopUp model)
        {
            if (ModelState.IsValid)
            {
                var result = await _topUpService.CreateAsync(model);

                if (result.IsSuccess)
                {
                    return Ok(result); //200
                }

                return BadRequest(result);
            }

            return BadRequest(
            new OperationResponse<TopUp>
            {
                IsSuccess = false,
                Message = "Internal server error"
            }); //400
        }

        [HttpPut("UpdateTopUp")]
        public async Task<IActionResult> UpdateAsync([FromBody] TopUp model)
        {
            if (string.IsNullOrWhiteSpace(model.ReferenceId) || string.IsNullOrWhiteSpace(model.Status))
            {
                return NotFound();
            }

            var result = await _topUpService.UpdateTopUp(model);

            if (result.IsSuccess)
            {
                return Ok(result); //200
            }

            return BadRequest(
            new OperationResponse<TopUp>
            {
                IsSuccess = false,
                Message = result.Message
            }) ; //400
        }
    }
}
