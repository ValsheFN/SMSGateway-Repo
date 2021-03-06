using EFCore.BulkExtensions;
using SMSGateway.Server.Infrastructure;
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
        Task<OperationResponse<ContactGroup>> ImportAsync(List<ContactGroup> model);
        Task<OperationResponse<ContactGroup>> UpdateAsync(ContactGroup model);
        Task<OperationResponse<ContactGroup>> RemoveAsync(string referenceId);
        Task<OperationResponse<ContactGroup>> RemoveByUserIdAsync(string userId);
        List<ContactGroup> GetAllFiltered(string userId, string referenceId, string groupName,
                                          string firstName, string lastName, string phoneNumber);
    }

    public class ContactGroupService : IContactGroupService
    {
        private readonly ApplicationDBContext _db;
        private readonly IdentityOption _identity;

        public ContactGroupService(ApplicationDBContext db, IdentityOption identity)
        {
            _db = db;
            _identity = identity;
        }
        public async Task<OperationResponse<ContactGroup>> CreateAsync(ContactGroup model)
        {
            var groupName = _db.ContactGroups.Where(x => x.GroupName == model.GroupName).ToList();

            if (groupName == null)
            {
                return new OperationResponse<ContactGroup>
                {
                    Message = "Group does not exist",
                    IsSuccess = false

                };
            }

            //var userId = User.Identity.GetUserId();
            var contactGroup = new ContactGroup
            {
                ContactId = model.ContactId,
                GroupName = model.GroupName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
            };

            await _db.ContactGroups.AddAsync(contactGroup);
            await _db.SaveChangesAsync(_identity.UserId);

            model.Id = contactGroup.Id;

            return new OperationResponse<ContactGroup>
            {
                Message = "Contact group created successfully!",
                IsSuccess = true,
                Data = model
            };
        }

        public async Task<OperationResponse<ContactGroup>> ImportAsync(List<ContactGroup> model)
        {
            _db.ContactGroups.AddRange(model);
            await _db.SaveChangesAsync(_identity.UserId);

            return new OperationResponse<ContactGroup>
            {
                Message = "Contact group imported successfully!",
                IsSuccess = true,
            };
        }

        public async Task<OperationResponse<ContactGroup>> UpdateAsync(ContactGroup model)
        {
            var contactGroup = _db.ContactGroups.SingleOrDefault(x => x.ReferenceId == model.ReferenceId);

            if (contactGroup == null)
            {
                return new OperationResponse<ContactGroup>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Contact group not found"
                };
            }

            contactGroup.GroupName = model.GroupName;

            _db.ContactGroups.Update(contactGroup);
            await _db.SaveChangesAsync(_identity.UserId);

            return new OperationResponse<ContactGroup>
            {
                Message = "Contact group updated successfully!",
                IsSuccess = true,
                Data = contactGroup
            };
        }

        public async Task<OperationResponse<ContactGroup>> RemoveAsync(string referenceId)
        {
            if (string.IsNullOrWhiteSpace(referenceId))
            {
                return new OperationResponse<ContactGroup>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Reference Id cannot be null!"
                };
            }

            var contactGroup = _db.ContactGroups.SingleOrDefault(x => x.ReferenceId == referenceId);

            if (contactGroup == null)
            {
                return new OperationResponse<ContactGroup>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Contact group not found!"
                };
            }

            _db.ContactGroups.Remove(contactGroup);
            await _db.SaveChangesAsync(_identity.UserId);

            return new OperationResponse<ContactGroup>
            {
                IsSuccess = true,
                Message = "Contact group has been deleted successfully!"
            };
        }

        public async Task<OperationResponse<ContactGroup>> RemoveByUserIdAsync(string contactId)
        {
            if (string.IsNullOrWhiteSpace(contactId))
            {
                return new OperationResponse<ContactGroup>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "ContactId Id cannot be null!"
                };
            }

            var contactGroup = _db.ContactGroups.SingleOrDefault(x => x.ContactId == contactId);

            if (contactGroup == null)
            {
                return new OperationResponse<ContactGroup>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Contact Id not found!"
                };
            }

            _db.ContactGroups.Remove(contactGroup);
            await _db.SaveChangesAsync(_identity.UserId);

            return new OperationResponse<ContactGroup>
            {
                IsSuccess = true,
                Message = "Contact group has been deleted successfully!"
            };
        }

        public List<ContactGroup> GetAllFiltered(string userId, string referenceId, string groupName, 
                                                string firstName, string lastName, string phoneNumber)
        {
            var query = _db.ContactGroups.ToList();

            query = query.Where(x => x.CreatedByUserId == _identity.UserId).ToList();

            if (userId != "" && userId != null)
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
