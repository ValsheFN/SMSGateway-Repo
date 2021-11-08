using Microsoft.AspNetCore.Identity;
using SMSGateway.Client.Models;
using SMSGateway.Server.Models;
using SMSGateway.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Server.Services
{
    public interface IRoleService
    {
        public List<IdentityRole> ListRoles();
        public Task<OperationResponse<Role>> CreateAsync(Role model);
        public Task<OperationResponse<Role>> UpdateAsync(Role model);
        public Task<OperationResponse<Role>> DeleteAsync(string id);
    }

    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public List<IdentityRole> ListRoles()
        {
            var roles = _roleManager.Roles.ToList();

            return roles;
        }

        public async Task<OperationResponse<Role>> CreateAsync(Role model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return new OperationResponse<Role>
                {
                    Message = "Role name cannot be empty",
                    IsSuccess = false,
                    Data = null
                };
            }

            if (await _roleManager.RoleExistsAsync(model.Name))
            {
                return new OperationResponse<Role>
                {
                    Message = "Role already exists",
                    IsSuccess = false,
                    Data = null
                };
            }

            await _roleManager.CreateAsync(new IdentityRole
            {
                Name = model.Name
            });

            return new OperationResponse<Role>
            {
                IsSuccess = true,
                Message = "Role created"
            };
        }

        public async Task<OperationResponse<Role>> UpdateAsync(Role model)
        {
            if (string.IsNullOrWhiteSpace(model.Id))
            {
                return new OperationResponse<Role>
                {
                    Message = "Role id cannot be empty",
                    IsSuccess = false,
                    Data = null
                };
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return new OperationResponse<Role>
                {
                    Message = "Role name cannot be empty",
                    IsSuccess = false,
                    Data = null
                };
            }

            if (await _roleManager.RoleExistsAsync(model.Name))
            {
                return new OperationResponse<Role>
                {
                    Message = "Role already exists",
                    IsSuccess = false,
                    Data = null
                };
            }

            var role = await _roleManager.FindByIdAsync(model.Id);
            role.Name = model.Name;
            var response = await _roleManager.UpdateAsync(role);

            if (response.Succeeded)
            {
                return new OperationResponse<Role>
                {
                    IsSuccess = true,
                    Message = "Role updated"
                };
            }
            else
            {
                return new OperationResponse<Role>
                {
                    IsSuccess = false,
                    Message = "Internal server error"
                };
            }
        }

        public async Task<OperationResponse<Role>> DeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new OperationResponse<Role>
                {
                    Message = "Role id cannot be empty",
                    IsSuccess = false,
                    Data = null
                };
            }

            var role = await _roleManager.FindByIdAsync(id);

            if(role.Name == "Super User" || role.Name == "Admin" || role.Name == "User")
            {
                return new OperationResponse<Role>
                {
                    IsSuccess = false,
                    Message = "Cannot delete role : Super User/Admin/User"
                };
            }

            var response = await _roleManager.DeleteAsync(role);

            if (response.Succeeded)
            {
                return new OperationResponse<Role>
                {
                    IsSuccess = true,
                    Message = "Role deleted"
                };
            }
            else
            {
                return new OperationResponse<Role>
                {
                    IsSuccess = false,
                    Message = "Internal server error"
                };
            }            
        }

    }
}
