using SMSGateway.Server.Models;
using SMSGateway.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Server.Services
{
    public interface IContactGroupService
    {
        Task<OperationResponse<ContactGroup>> CreateAsync(ContactGroup model);
        List<ContactGroup> GetAllFiltered(string userId, string referenceId, string FirstName, string LastName, string CreatedByUserId);
    }

    public class ContactGroupService : IContactGroupService
    {
        private readonly ApplicationDBContext _db;

        public ContactGroupService(ApplicationDBContext db)
        {
            _db = db;
        }
        public Task<OperationResponse<ContactGroup>> CreateAsync(ContactGroup model)
        {
            throw new NotImplementedException();
        }

        public List<ContactGroup> GetAllFiltered(string userId, string referenceId, string FirstName, string LastName, string CreatedByUserId)
        {
            throw new NotImplementedException();
        }
    }
}
