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
    public class SmsTemplateController : ControllerBase
    {
        private readonly ISmsTemplateService _smsTemplateService;

        public SmsTemplateController(ISmsTemplateService smsTemplateService)
        {
            _smsTemplateService = smsTemplateService;
        }

        [HttpGet("GetSmsTemplate")]
        public async Task<IActionResult> GetAllFiltered(string referenceId, string smsTemplateName, string content)
        {
            return Ok(_smsTemplateService.GetAllFiltered(referenceId, smsTemplateName, content));
        }

        [HttpPost("CreateSmsTemplate")]
        public async Task<IActionResult> CreateAsync([FromBody] SmsTemplate model)
        {
            if (ModelState.IsValid)
            {
                var result = await _smsTemplateService.CreateAsync(model);

                if (result.IsSuccess)
                {
                    return Ok(result); //200
                }

                return BadRequest(result);
            }

            return BadRequest(
            new OperationResponse<SmsTemplate>
            {
                IsSuccess = false,
                Message = "Internal server error"
            }); //400
        }

        [HttpPut("UpdateSmsTemplate")]
        public async Task<IActionResult> UpdateAsync([FromBody] SmsTemplate model)
        {
            if (ModelState.IsValid)
            {
                var result = await _smsTemplateService.UpdateAsync(model);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest(
            new OperationResponse<SmsTemplate>
            {
                IsSuccess = false,
                Message = "Internal server error"
            }); //400
        }

        [HttpDelete("RemoveSmsTemplate/{referenceId}")]
        public async Task<IActionResult> RemoveAsync(string referenceId)
        {
            if (ModelState.IsValid)
            {
                var result = await _smsTemplateService.RemoveAsync(referenceId);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest(
            new OperationResponse<SmsTemplate>
            {
                IsSuccess = false,
                Message = "Internal server error"
            }); //400
        }
    }
}
