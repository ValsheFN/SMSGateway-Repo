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
    [Authorize]
    public class ContactGroupController : ControllerBase
    {
        private readonly IContactGroupService _contactGroupService;

        public ContactGroupController(IContactGroupService contactGroupService)
        {
            _contactGroupService = contactGroupService;
        }

        [HttpGet("GetContactGroup")]
        public async Task<IActionResult> GetAllFiltered(string userId, string referenceId, string FirstName, string LastName, string CreatedByUserId)
        {
            var result = _contactGroupService.GetAllFiltered(userId, referenceId, FirstName, LastName, CreatedByUserId);

            if (result != null)
            {
                return Ok(result); //200
            }

            return BadRequest(result);
        }

        [HttpPost("CreateContactGroup")]
        public async Task<IActionResult> CreateContactGroup([FromBody]ContactGroup model)
    }
}
