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
    [Authorize]
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
            var result = _topUpService.GetAllFiltered(referenceId, requester, status,
                                          requestDateStart, requestDateEnd,
                                          grantDateStart, grantDateEnd,
                                          grantedBy);

            if (result.IsSuccess)
            {
                return Ok(result); //200
            }

            return BadRequest(result);
        }

        [HttpPost("CreateTopUp")]
        public async Task<IActionResult> CreateAsync([FromForm] TopUp model)
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

            return BadRequest("Some properties are not valid"); //400
        }

        [HttpGet("UpdateTopUp")]
        public async Task<IActionResult> UpdateTopUp(string referenceId, string action)
        {
            if (string.IsNullOrWhiteSpace(referenceId) || string.IsNullOrWhiteSpace(action))
            {
                return NotFound();
            }

            var result = await _topUpService.UpdateTopUp(referenceId, action);

            if (result.IsSuccess)
            {
                return Ok(result); //200
            }

            return BadRequest(result);
        }
    }
}
