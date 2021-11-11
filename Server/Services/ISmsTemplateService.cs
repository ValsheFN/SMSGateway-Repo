using SMSGateway.Server.Infrastructure;
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
        Task<OperationResponse<SmsTemplate>> UpdateAsync(SmsTemplate model);
        Task<OperationResponse<SmsTemplate>> RemoveAsync(string referenceId);
        List<SmsTemplate> GetAllFiltered(string referenceId, string smsTemplateName, string content);
    }

    public class SmsTemplateService : ISmsTemplateService
    {
        private readonly ApplicationDBContext _db;
        private readonly IdentityOption _identity;

        public SmsTemplateService(ApplicationDBContext db, IdentityOption identity)
        {
            _db = db;
            _identity = identity;
        }

        public async Task<OperationResponse<SmsTemplate>> CreateAsync(SmsTemplate model)
        {
            var query = _db.SmsTemplates.ToList();
            query = query.Where(x => x.SmsTemplateName == model.SmsTemplateName).ToList();

            if(query.Count > 0)
            {
                return new OperationResponse<SmsTemplate>
                {
                    Message = "Please use different template name!",
                    IsSuccess = false
                };
            }


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
                Message = "Sms template created successfully!",
                IsSuccess = true,
                Data = model
            };
        }

        public async Task<OperationResponse<SmsTemplate>> UpdateAsync(SmsTemplate model)
        {
            var smsTemplate = _db.SmsTemplates.SingleOrDefault(x => x.ReferenceId == model.ReferenceId);

            if (smsTemplate == null)
            {
                return new OperationResponse<SmsTemplate>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Sms template not found"
                };
            }

            var query = _db.SmsTemplates.ToList();
            query = query.Where(x => x.SmsTemplateName == model.SmsTemplateName).ToList();

            if (query.Count > 0)
            {
                return new OperationResponse<SmsTemplate>
                {
                    Message = "Please use different template name!",
                    IsSuccess = false
                };
            }

            smsTemplate.SmsTemplateName = model.SmsTemplateName;
            smsTemplate.Content = model.Content;

            _db.SmsTemplates.Update(smsTemplate);
            await _db.SaveChangesAsync(_identity.UserId);

            return new OperationResponse<SmsTemplate>
            {
                Message = "Sms template updated successfully!",
                IsSuccess = true,
                Data = smsTemplate
            };
        }

        public async Task<OperationResponse<SmsTemplate>> RemoveAsync(string referenceId)
        {
            var template = _db.SmsTemplates.SingleOrDefault(x => x.ReferenceId == referenceId);

            if (template == null)
            {
                return new OperationResponse<SmsTemplate>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Sms template not found"
                };
            }

            _db.SmsTemplates.Remove(template);
            await _db.SaveChangesAsync(_identity.UserId);

            return new OperationResponse<SmsTemplate>
            {
                IsSuccess = true,
                Message = "Sms template has been deleted successfully"
            };
        }

        public List<SmsTemplate> GetAllFiltered(string referenceId, string smsTemplateName, string content)
        {
            var query = _db.SmsTemplates.ToList();

            if (referenceId != "" && referenceId != null)
            {
                query = query.Where(x => x.ReferenceId == referenceId).ToList();
            }
            if (smsTemplateName != "" && smsTemplateName != null)
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
