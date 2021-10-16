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
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet("FilteredGroup")]
        public async Task<IActionResult> GetGroupFiltered(string referenceId, string groupName, string createdByUserId)
        {
            return Ok(_groupService.GetAllFiltered(referenceId, groupName, createdByUserId));
        }

        [HttpPost("GroupCreation")]
        public async Task<IActionResult> PostGroup([FromBody] Group model)
        {
            if (ModelState.IsValid)
            {
                var result = await _groupService.CreateAsync(model);

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
