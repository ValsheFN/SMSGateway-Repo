using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SMSGateway.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMSGateway.Server.Models;
using SMSGateway.Shared;

namespace SMSGateway.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet()]
        public async Task<IActionResult> ListUsers()
        {
            return Ok(_userService.ListUsers());
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateUser([FromBody] User model)
        { 
            if (ModelState.IsValid)
            {
                var result = await _userService.UpdateUser(model);

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

        [HttpDelete()]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest(
                new OperationResponse<Contact>
                {
                    IsSuccess = false,
                    Message = "User Id cannot be null"
                });
            }

            var result = await _userService.DeleteUser(userId);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
