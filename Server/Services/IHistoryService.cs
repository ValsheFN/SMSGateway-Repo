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
        Task<OperationResponse<History>> BulkUpdateAsync(List<History> model);
        List<History> GetAllFiltered(string referenceId, string status, bool hasMessageId);
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
                MessageId = model.MessageId,
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
        public List<History> GetAllFiltered(string referenceId, string status, bool hasMessageId)
        {
            var query = _db.History.ToList();

            if (!string.IsNullOrWhiteSpace(referenceId))
            {
                query = query.Where(x => x.ReferenceId == referenceId).ToList();
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(x => x.Status == status).ToList();
            }

            if (hasMessageId)
            {
                query = query.Where(x => x.MessageId != null).ToList();
            }

            return query;
        }

        public async Task<OperationResponse<History>> BulkUpdateAsync(List<History> model)
        {
            var count = model.Count();

            if(count <= 0)
            {
                return new OperationResponse<History>
                {
                    Message = "No data found",
                    IsSuccess = false
                };
            }

            foreach(var data in model)
            {
                await _db.History.AddAsync(data);
            }

            try
            {
                await _db.SaveChangesAsync();

                return new OperationResponse<History>
                {
                    Message = "SMS history updated",
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new OperationResponse<History>
                {
                    Message = $"Error - {e.InnerException.ToString()}",
                    IsSuccess = false
                };
            }

            

        }
    }
}

    
