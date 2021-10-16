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

        [HttpGet("FilteredContact")]
        public async Task<IActionResult> GetContactFiltered(string userId, string referenceId, string firstName, string lastName, string createdByUserId)
        {
            return Ok(_contactService.GetAllFiltered(userId, "", "", "", ""));
        }

        [HttpPost("ContactCreation")]
        public async Task<IActionResult> PostContact([FromBody] Contact model)
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

            return BadRequest("Internal Server Error"); //400
        }
    }
}
