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
    public class ContactGroupController : ControllerBase
    {
        private readonly IContactGroupService _contactGroupService;

        public ContactGroupController(IContactGroupService contactGroupService)
        {
            _contactGroupService = contactGroupService;
        }

        [HttpGet("GetContactGroup")]
        public async Task<IActionResult> GetAllFiltered(string userId, string referenceId, string groupName,
                                                        string firstName, string lastName, string phoneNumber,
                                                        string createdByUserId)
        {
            return Ok(_contactGroupService.GetAllFiltered(userId, referenceId, groupName, firstName, lastName, phoneNumber, createdByUserId));
        }

        [HttpPost("CreateContactGroup")]
        public async Task<IActionResult> CreateContactGroup([FromBody]ContactGroup model)
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
            return BadRequest("Internal Server Error"); //400
        }
    }
}
