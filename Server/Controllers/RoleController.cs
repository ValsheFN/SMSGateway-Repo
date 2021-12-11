using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // /api/role
        [HttpGet()]
        public async Task<IActionResult> ListRoles()
        {
            return Ok(_roleService.ListRoles());
        }

        [HttpPost()]
        public async Task<IActionResult> CreateAsync(Role model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.CreateAsync(model);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest("Internal Server Error"); //400
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateAsync(Role model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.UpdateAsync(model);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest("Internal Server Error"); //400
        }

        [HttpPut("UpdateUserRole")]
        public async Task<IActionResult> UpdateUserRole(string id, string newRole)
        {
            var result = await _roleService.UpdateUserRole(id, newRole);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete()]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var result = await _roleService.DeleteAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
