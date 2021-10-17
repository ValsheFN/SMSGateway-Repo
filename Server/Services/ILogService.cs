using SMSGateway.Server.Models;
using SMSGateway.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Server.Services
{
    public interface ILogService
    {
        Task<OperationResponse<Log>> CreateAsync(Log model);
        List<Log> GetAllFiltered(string from, string sendTo, string messages, DateTime TimeStart, DateTime TimeEnd);
    }

    public class LogService : ILogService
    {
        private readonly ApplicationDBContext _db;

        public LogService(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task<OperationResponse<Log>> CreateAsync(Log model)
        {
            var log = new Log
            {
                From = model.From,
                SendTo = model.SendTo,
                Messages = model.Messages,
                TimeSent = model.TimeSent
            };

            await _db.Logs.AddAsync(log);
            await _db.SaveChangesAsync();

            model.Id = log.Id;

            return new OperationResponse<Log>
            {
                Message = "Contact created successfully!",
                IsSuccess = true,
                Data = model
            };
        }

        public List<Log> GetAllFiltered(string from, string sendTo, string messages, DateTime timeStart, DateTime timeEnd)
        {
            var query = _db.Logs.ToList();

            if(from != "" && from != null)
            {
                query = query.Where(x => x.From == from).ToList();
            }

            if (from != "" && from != null)
            {
                query = query.Where(x => x.From == from).ToList();
            }

            if (from != "" && from != null)
            {
                query = query.Where(x => x.From == from).ToList();
            }

            if (from != "" && from != null)
            {
                query = query.Where(x => x.From == from).ToList();
            }

            return query;
        }
    }
}
