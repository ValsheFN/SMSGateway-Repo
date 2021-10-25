using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMSGateway.Server.Models;
using SMSGateway.Server.Services;
using SMSGateway.Shared;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;

        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }
        // /api/mail
        [HttpPost]
        public async Task<UserManagerResponse> SendEmailAsync([FromBody] Mail model)
        {
            var response = await _mailService.SendEmailAsync(model.SendTo, model.From, model.Subject, model.Content);

            if(response != "Success")
            {
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = response
                };
            }
            else
            {
                return new UserManagerResponse
                {
                    IsSuccess = true,
                    Message = response
                };
            }
        }
    }
}
