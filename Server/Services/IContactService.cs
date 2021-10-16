using System;
using System.Collections.Generic;
using System.Linq;
using SMSGateway.Server.Models;
using SMSGateway.Shared;
using System.Threading.Tasks;
using SMSGateway.Server.Infrastructure;

namespace SMSGateway.Server.Services
{
    public interface IContactService
    {
        Task<OperationResponse<Contact>> CreateAsync(Contact model);
        List<Contact> GetAllFiltered(string userId, string referenceId, string FirstName, string LastName, string CreatedByUserId);
    }

    public class ContactService : IContactService
    {
        private readonly ApplicationDBContext _db;

        public ContactService(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task<OperationResponse<Contact>> CreateAsync(Contact model)
        {
            //var userId = User.Identity.GetUserId();
            var contact = new Contact
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Notes = model.Notes
            };

            await _db.Contact.AddAsync(contact);
            await _db.SaveChangesAsync();

            model.Id = contact.Id;

            return new OperationResponse<Contact>
            {
                IsSuccess = true,
                Message = "Contact created successfully!",
                Data = model
            };
        }

        public List<Contact> GetAllFiltered(string userId, string referenceId, string FirstName, string LastName, string CreatedByUserId)
        {
            return _db.Contact.Where(x => x.CreatedByUserId == userId).ToList();
        }
    }
}
