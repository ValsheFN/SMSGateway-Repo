using SMSGateway.Server.Models;
using SMSGateway.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Server.Services
{
    public interface ISmsTemplateService
    {
        Task<OperationResponse<SmsTemplate>> CreateAsync(SmsTemplate model);
        List<SmsTemplate> GetAllFiltered(string smsTemplateName, string content);
    }

    public class SmsTemplateService : ISmsTemplateService
    {
        private readonly ApplicationDBContext _db;

        public SmsTemplateService(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task<OperationResponse<SmsTemplate>> CreateAsync(SmsTemplate model)
        {
            var smsTemplate = new SmsTemplate
            {
                SmsTemplateName = model.SmsTemplateName,
                Content = model.Content
            };

            await _db.SmsTemplates.AddAsync(smsTemplate);
            await _db.SaveChangesAsync();

            model.Id = smsTemplate.Id;

            return new OperationResponse<SmsTemplate>
            {
                Message = "Contact created successfully!",
                IsSuccess = true,
                Data = model
            };
        }

        public List<SmsTemplate> GetAllFiltered(string smsTemplateName, string content)
        {
            var query = _db.SmsTemplates.ToList();

            if(smsTemplateName != "" && smsTemplateName != null)
            {
                query = query.Where(x => x.SmsTemplateName == smsTemplateName).ToList();
            }

            if (content != "" && content != null)
            {
                query = query.Where(x => x.Content.Contains(content)).ToList();
            }

            return query;
        }
    }
}
