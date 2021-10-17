using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMSGateway.Server.Models;
using SMSGateway.Server.Services;
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
        public async Task<IActionResult> GetAllFiltered(string smsTemplateName, string content)
        {
            return Ok(_smsTemplateService.GetAllFiltered(smsTemplateName, content));
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

            return BadRequest("Some properties are not valid"); //400
        }
    }
}
