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
        List<ContactGroup> GetAllFiltered(string userId, string referenceId, string groupName,
                                          string firstName, string lastName, string phoneNumber,
                                          string createdByUserId);
    }

    public class ContactGroupService : IContactGroupService
    {
        private readonly ApplicationDBContext _db;

        public ContactGroupService(ApplicationDBContext db)
        {
            _db = db;
        }
        public async Task<OperationResponse<ContactGroup>> CreateAsync(ContactGroup model)
        {
            var groupName = _db.Groups.Where(x => x.GroupName == model.GroupName).ToList();
            if (groupName == null)
            {
                return new OperationResponse<ContactGroup>
                {
                    Message = "Group does not exist",
                    IsSuccess = false

                };
            }

            var phoneNumber = _db.Contact.Where(x => x.PhoneNumber == model.PhoneNumber).ToList();
            if(phoneNumber == null)
            {
                return new OperationResponse<ContactGroup>
                {
                    Message = "Contact does not exist",
                    IsSuccess = false

                };
            }

            //var userId = User.Identity.GetUserId();
            var contactGroup = new ContactGroup
            {
                GroupName = model.GroupName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
            };

            await _db.ContactGroups.AddAsync(contactGroup);
            await _db.SaveChangesAsync();

            model.Id = contactGroup.Id;

            return new OperationResponse<ContactGroup>
            {
                Message = "Contact created successfully!",
                IsSuccess = true,
                Data = model
            };
        }

        public List<ContactGroup> GetAllFiltered(string userId, string referenceId, string groupName, 
                                                string firstName, string lastName, string phoneNumber,
                                                string createdByUserId)
        {
            var query = _db.ContactGroups.ToList();

            if (createdByUserId != "" && createdByUserId != null)
            {
                query = query.Where(x => x.CreatedByUserId == createdByUserId).ToList();
            }

            if(userId != "" && userId != null)
            {
                query = query.Where(x => x.Id == userId).ToList();
            }

            if(referenceId != "" && referenceId != null)
            {
                query = query.Where(x => x.ReferenceId == userId).ToList();
            }

            if (groupName != "" && groupName != null)
            {
                query = query.Where(x => x.GroupName == groupName).ToList();
            }

            if (firstName != "" && firstName != null)
            {
                query = query.Where(x => x.FirstName == firstName).ToList();
            }

            if (lastName != "" && lastName != null)
            {
                query = query.Where(x => x.LastName == lastName).ToList();
            }

            if (phoneNumber != "" && phoneNumber != null)
            {
                query = query.Where(x => x.PhoneNumber == phoneNumber).ToList();
            }

            return query;
        }
    }
}
