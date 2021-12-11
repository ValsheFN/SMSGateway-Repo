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
    public class ContactGroupController : ControllerBase
    {
        private readonly IContactGroupService _contactGroupService;

        public ContactGroupController(IContactGroupService contactGroupService)
        {
            _contactGroupService = contactGroupService;
        }

        [HttpGet("GetContactGroup")]
        public async Task<IActionResult> GetAllFiltered(string userId, string referenceId, string groupName,
                                                        string firstName, string lastName, string phoneNumber)
        {
            return Ok(_contactGroupService.GetAllFiltered(userId, referenceId, groupName, firstName, lastName, phoneNumber));
        }

        [HttpPost("CreateContactGroup")]
        public async Task<IActionResult> CreateAsync([FromBody]ContactGroup model)
        {
            if (ModelState.IsValid)
            {
                var result = await _contactGroupService.CreateAsync(model);

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

        [HttpPost("ImportContactGroup")]
        public async Task<IActionResult> ImportAsync([FromBody] List<ContactGroup> model)
        {
            var result = await _contactGroupService.ImportAsync(model);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPut("UpdateContactGroup")]
        public async Task<IActionResult> UpdateAsync([FromBody] ContactGroup model)
        {
            if (ModelState.IsValid)
            {
                var result = await _contactGroupService.UpdateAsync(model);

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

        [HttpDelete("RemoveContactGroup")]
        public async Task<IActionResult> RemoveAsync(string referenceId)
        {
            var result = await _contactGroupService.RemoveAsync(referenceId);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("RemoveByUserIdAsync")]
        public async Task<IActionResult> RemoveByUserIdAsync(string contactId)
        {
            var result = await _contactGroupService.RemoveByUserIdAsync(contactId);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
