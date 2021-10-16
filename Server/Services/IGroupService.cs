﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Exchange.WebServices.Data;
using SMSGateway.Server.Models;
using SMSGateway.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Server.Services
{
    public interface IGroupService
    {
        Task<OperationResponse<Group>> CreateAsync(Group model);
        List<Group> GetAllFiltered(string referenceId, string groupName, string createdByUserId);
    }

    public class GroupService : IGroupService
    {
        private readonly ApplicationDBContext _db;
        public GroupService(ApplicationDBContext db)
        {
            _db = db;
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

        public List<Group> GetAllFiltered(string referenceId, string groupName, string createdByUserId)
{
            return _db.Groups.Where(x => x.ReferenceId == referenceId)
                             .Include(x => x.GroupName == groupName)
                             .Include(x => x.CreatedByUserId == createdByUserId)
                             .ToList();
        }
    }

}
