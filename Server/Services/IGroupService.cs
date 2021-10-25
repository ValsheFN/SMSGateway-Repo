using Microsoft.EntityFrameworkCore;
using Microsoft.Exchange.WebServices.Data;
using SMSGateway.Server.Infrastructure;
using SMSGateway.Server.Models;
using SMSGateway.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace SMSGateway.Server.Services
{
    public interface IGroupService
    {
        Task<OperationResponse<Group>> CreateAsync(Group model);
        Task<OperationResponse<Group>> UpdateAsync(Group model);
        Task<OperationResponse<Group>> RemoveAsync(string referenceId);
        List<Group> GetAllFiltered(string referenceId, string groupName, string createdByUserId);
    }

    public class GroupService : IGroupService
    {
        private readonly ApplicationDBContext _db;
        private readonly IdentityOption _identity;
        public GroupService(ApplicationDBContext db, IdentityOption identity)
        {
            _db = db;
            _identity = identity;
        }
        public async Task<OperationResponse<Group>> CreateAsync(Group model)
        {
            var group = new Group
            {
                GroupName = model.GroupName
            };

            await _db.Groups.AddAsync(group);
            await _db.SaveChangesAsync();

            model.Id = group.Id;

            return new OperationResponse<Group>
            {
                Message = "Group created successfully!",
                IsSuccess = true,
                Data = model
            };
        }

        public async Task<OperationResponse<Group>> UpdateAsync(Group model)
        {
            var group = _db.Groups.SingleOrDefault(x => x.ReferenceId == model.ReferenceId);

            if (group == null)
            {
                return new OperationResponse<Group>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Group not found"
                };
            }

            group.GroupName = model.GroupName;

            await _db.Groups.AddAsync(group);
            await _db.SaveChangesAsync(_identity.UserId);

            return new OperationResponse<Group>
            {
                Message = "Group updated successfully!",
                IsSuccess = true,
                Data = group
            };
        }

        public async Task<OperationResponse<Group>> RemoveAsync(string referenceId)
        {
            var group = _db.Groups.SingleOrDefault(x => x.ReferenceId == referenceId);

            if (group == null)
            {
                return new OperationResponse<Group>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Group not found"
                };
            }

            _db.Groups.Remove(group);
            await _db.SaveChangesAsync(_identity.UserId);

            return new OperationResponse<Group>
            {
                IsSuccess = true,
                Message = "Group has been deleted successfully"
            };
        }

        public List<Group> GetAllFiltered(string referenceId, string groupName, string createdByUserId)
{
            var query = _db.Groups.ToList();

            if (createdByUserId != "" && createdByUserId != null)
            {
                query = query.Where(x => x.CreatedByUserId == createdByUserId).ToList();
            }

            if(referenceId != "" && referenceId != null)
            {
                query = query.Where(x => x.ReferenceId == referenceId).ToList();
            }

            if (groupName != "" && groupName != null)
            {
                query = query.Where(x => x.GroupName == groupName).ToList();
            }

            return query;
        }
    }

}
