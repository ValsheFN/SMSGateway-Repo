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
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet("GetContact")]
        public async Task<IActionResult> GetContactFiltered(string userId, string referenceId, string firstName, string lastName, string contactGroupId)
        {
            return Ok(_contactService.GetAllFiltered(userId, referenceId, firstName, lastName, contactGroupId));
        }

        [HttpPost("CreateContact")]
        public async Task<IActionResult> CreateAsync([FromBody] Contact model)
        {
            if (ModelState.IsValid)
            {
                var result = await _contactService.CreateAsync(model);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest(
            new OperationResponse<Contact>
            {
                IsSuccess = false,
                Message = "Internal server error"
            }); //400
        }

        [HttpPut("UpdateContact")]
        public async Task<IActionResult> UpdateAsync([FromBody] Contact model)
        {
            if (ModelState.IsValid)
            {
                var result = await _contactService.UpdateAsync(model);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest(
            new OperationResponse<Contact>
            {
                IsSuccess = false,
                Message = "Internal server error"
            }); //400
        }

        [HttpDelete("RemoveContact/{referenceId}")]
        public async Task<IActionResult> RemoveAsync(string referenceId)
        {
            if (ModelState.IsValid)
            {
                var result = await _contactService.RemoveAsync(referenceId);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest(
            new OperationResponse<Contact>
            {
                IsSuccess = false,
                Message = "Internal server error"
            }); //400
        }
    }
}
