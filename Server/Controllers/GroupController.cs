using Microsoft.AspNetCore.Authorization;
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
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet("GetGroup")]
        public async Task<IActionResult> GetGroupFiltered(string referenceId, string groupName, string createdByUserId)
        {
            return Ok(_groupService.GetAllFiltered(referenceId, groupName, createdByUserId));
        }

        [HttpPost("CreateGroup")]
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
            return BadRequest(
            new OperationResponse<Group>
            {
                IsSuccess = false,
                Message = "Internal server error"
            }); //400
        }

        [HttpPut("UpdateGroup")]
        public async Task<IActionResult> UpdateAsync([FromBody] Group model)
        {
            if (ModelState.IsValid)
            {
                var result = await _groupService.UpdateAsync(model);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest(
            new OperationResponse<Group>
            {
                IsSuccess = false,
                Message = "Internal server error"
            }); //400
        }

        [HttpDelete("RemoveGroup/{referenceId}")]
        public async Task<IActionResult> RemoveContactGroup(string referenceId)
        {
            if (ModelState.IsValid)
            {
                var result = await _groupService.RemoveAsync(referenceId);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest(
            new OperationResponse<Group>
            {
                IsSuccess = false,
                Message = "Internal server error"
            }); //400
        }
    }
}
