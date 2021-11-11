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
    public interface IHistoryService
    {
        Task<OperationResponse<History>> CreateAsync(History model);
        Task<OperationResponse<History>> UpdateAsync(string referenceId, string status);
        List<History> GetAllFiltered(string referenceId, string createdByUserId);
    }

    public class HistoryService : IHistoryService
    {
        private readonly ApplicationDBContext _db;
        private readonly IdentityOption _identity;

        public HistoryService(ApplicationDBContext db, IdentityOption identity)
        {
            _db = db;
            _identity = identity;
        }

        public async Task<OperationResponse<History>> CreateAsync(History model)
        {
            var history = new History
            {
                Recipients = model.Recipients,
                Messages = model.Messages,
                TimeSent = model.TimeSent,
                Status = model.Status
            };

            try
            {
                await _db.History.AddAsync(history);
                await _db.SaveChangesAsync(_identity.UserId);

                model.Id = history.Id;

                return new OperationResponse<History>
                {
                    Message = "History created successfully!",
                    IsSuccess = true,
                    Data = model
                };
            }
            catch(Exception e)
            {
                return new OperationResponse<History>
                {
                    Message = "Failed to create history! - " + e.Message,
                    IsSuccess = false,
                    Data = model
                };
            }
            
        }
        public async Task<OperationResponse<History>> UpdateAsync(string referenceId, string status)
        {
            var history = _db.History.SingleOrDefault(x => x.ReferenceId == referenceId);

            if (history == null)
            {
                return new OperationResponse<History>
                {
                    IsSuccess = false,
                    Message = "Reference id not found"
                };
            }

            history.Status = status;

            try
            {
                await _db.SaveChangesAsync(_identity.UserId);

                return new OperationResponse<History>
                {
                    Message = "History updated successfully!",
                    IsSuccess = true,
                    Data = history
                };
            }
            catch(Exception e)
            {
                return new OperationResponse<History>
                {
                    Message = "History failed to be updated! - " + e.Message ,
                    IsSuccess = false,
                    Data = history
                };
            }
            
        }
        public List<History> GetAllFiltered(string referenceId, string createdByUserId)
        {
            var query = _db.History.ToList();

            if (!string.IsNullOrWhiteSpace(referenceId))
            {
                query = query.Where(x => x.ReferenceId == referenceId).ToList();
            }

            if (!string.IsNullOrEmpty(createdByUserId))
            {
                query = query.Where(x => x.CreatedByUserId == createdByUserId).ToList();
            }

            return query;
        }
    }
}

    
