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
       /* Task<UserManagerResponse> UpdateAsync(string referenceId, Contact model);*/
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
            var userPhoneNumber = _db.Contact.Where(x => x.PhoneNumber == model.PhoneNumber).ToList();
            if(userPhoneNumber != null)
            {
                return new OperationResponse<Contact>
                {
                    Message = "Phoone number is already exists",
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
            await _db.SaveChangesAsync();

            model.Id = contact.Id;

            return new OperationResponse<Contact>
            {
                Message = "Contact created successfully!",
                IsSuccess = true,
                Data = model
            };
        }

        public List<Contact> GetAllFiltered(string userId, string referenceId, string FirstName, string LastName, string CreatedByUserId)
        {
            return _db.Contact.Where(x => x.CreatedByUserId == userId).ToList();
        }

        /*public async Task<UserManagerResponse> UpdateAsync(string referenceId, Contact model)
        {
            //Check if contact exist
            var existingContact = _db.Contact.Find(referenceId);

            if(existingContact == null)
            {
                return new UserManagerResponse
                {
                    Message = "Contact is not found",
                    IsSuccess = true
                };
            }

            var contact = new Contact
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Notes = model.Notes
            };

            var result = await _db.Contact.AddAsync(referenceId, model);
            if (result.Succeeded)
            {
                return new UserManagerResponse
                {
                    Message = "Contact has been updated",
                    IsSuccess = true
                };
            }

            return new UserManagerResponse
            {
                Message = "Contact is not updated",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }*/
    }
}
