using System;
using System.Collections.Generic;
using System.Linq;
using SMSGateway.Server.Models;
using SMSGateway.Shared;
using System.Threading.Tasks;
using SMSGateway.Server.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Data.Services.Client;

namespace SMSGateway.Server.Services
{
    public interface IContactService
    {
        Task<OperationResponse<Contact>> CreateAsync(Contact model);
        Task<OperationResponse<Contact>> UpdateAsync(Contact model);
        Task<OperationResponse<Contact>> RemoveAsync(Contact model);
        List<Contact> GetAllFiltered(string userId, string referenceId, string firstName, string lastName, string createdByUserId);

        Task CommitChangesAsync(string userId);
    }

    public class ContactService : IContactService
    {
        private readonly ApplicationDBContext _db;
        private readonly IdentityOption _identity;

        public ContactService(ApplicationDBContext db, IdentityOption identity)
        {
            _db = db;
            _identity = identity;
        }

        public async Task CommitChangesAsync(string userId)
        {
            await _db.SaveChangesAsync(userId);
        }

        public async Task<OperationResponse<Contact>> CreateAsync(Contact model)
{
            var userPhoneNumber = _db.Contact.Where(x => x.PhoneNumber == model.PhoneNumber).ToList();
            if(userPhoneNumber.Count != 0)
            {
                return new OperationResponse<Contact>
                {
                    Message = "Phone number is already exists",
                    IsSuccess = false

                };
            }
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
            await _db.SaveChangesAsync(_identity.UserId);

            model.Id = contact.Id;

            return new OperationResponse<Contact>
            {
                Message = "Contact created successfully!",
                IsSuccess = true,
                Data = model
            };
        }

        public async Task<OperationResponse<Contact>> UpdateAsync(Contact model)
        {
            var oldContact = _db.Contact.SingleOrDefault(x => x.ReferenceId == model.ReferenceId);

            if(oldContact == null)
            {
                return new OperationResponse<Contact>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Contact not found"
                };
            }

            var newContact = new Contact
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Notes = model.Notes
            };

            await _db.Contact.AddAsync(newContact);
            await _db.SaveChangesAsync(_identity.UserId);

            model.Id = newContact.Id;

            return new OperationResponse<Contact>
            {
                Message = "Contact updated successfully!",
                IsSuccess = true,
                Data = model
            };
        }

        public async Task<OperationResponse<Contact>> RemoveAsync(Contact model)
        {
            var contact = _db.Contact.SingleOrDefault(x => x.ReferenceId == model.ReferenceId);

            if (contact == null)
            {
                return new OperationResponse<Contact>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Contact not found"
                };
            }

            _db.Contact.Remove(contact);
            await _db.SaveChangesAsync(_identity.UserId);

            return new OperationResponse<Contact>
            {
                IsSuccess = true,
                Message = "Contact has been deleted successfully"
            };
        }

        public List<Contact> GetAllFiltered(string userId, string referenceId, string firstName, string lastName, string createdByUserId)
        {
            var createdBy = _identity.UserId;

            var query = _db.Contact.ToList();

            if (createdBy != "" && createdBy != null)
            {
                query = query.Where(x => x.CreatedByUserId == createdBy).ToList();
            }

            if (userId != "" && userId != null)
            {
                query = query.Where(x => x.Id == userId).ToList();
            }

            if (referenceId != "" && referenceId != null)
            {
                query = query.Where(x => x.ReferenceId == referenceId).ToList();
            }

            if (firstName != "" && firstName != null)
            {
                query = query.Where(x => x.FirstName == firstName).ToList();
            }

            if (lastName != "" && lastName != null)
            {
                query = query.Where(x => x.LastName == lastName).ToList();
            }
            return query;
        }
    }
}
