using System;
using System.Collections.Generic;
using System.Linq;
using SMSGateway.Server.Models;
using SMSGateway.Shared;
using System.Threading.Tasks;
using SMSGateway.Server.Infrastructure;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.IO;

namespace SMSGateway.Server.Services
{
    public interface IContactService
    {
        Task<OperationResponse<Contact>> CreateAsync(Contact model);
        Task<OperationResponse<Contact>> UpdateAsync(Contact model);
        Task<OperationResponse<Contact>> RemoveAsync(string referenceId);
        List<Contact> GetAllFiltered(string userId, string referenceId, string firstName, string lastName, string contactGroupId);
        Task<ExcelImportResponse<Contact>> ImportContactAsync(IFormFile formFile);
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
            var userPhoneNumber = _db.Contact.Where(x => x.PhoneNumber == model.PhoneNumber && x.CreatedByUserId == _identity.UserId).ToList();
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

            try
            {
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
            catch(Exception e)
            {
                model.Id = contact.Id;

                return new OperationResponse<Contact>
                {
                    Message = e.Message.ToString() + " " + e.InnerException.ToString(),
                    IsSuccess = false,
                    Data = model
                };
            }
            
        }

        public async Task<OperationResponse<Contact>> UpdateAsync(Contact model)
        {
            var contact = _db.Contact.SingleOrDefault(x => x.ReferenceId == model.ReferenceId && x.CreatedByUserId == _identity.UserId);

            if(contact == null)
            {
                return new OperationResponse<Contact>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Contact not found"
                };
            }

            contact.FirstName = model.FirstName;
            contact.LastName = model.LastName;
            contact.Email = model.Email;
            contact.PhoneNumber = model.PhoneNumber;
            contact.Notes = model.Notes;

            await _db.SaveChangesAsync(_identity.UserId);

            return new OperationResponse<Contact>
            {
                Message = "Contact updated successfully!",
                IsSuccess = true,
                Data = model
            };
        }

        public async Task<OperationResponse<Contact>> RemoveAsync(string referenceId)
        {
            var contact = _db.Contact.SingleOrDefault(x => x.ReferenceId == referenceId && x.CreatedByUserId == _identity.UserId);

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

        public List<Contact> GetAllFiltered(string userId, string referenceId, string firstName, string lastName, string contactGroupId)
        {
            var query = _db.Contact.ToList();

            query = query.Where(x => x.CreatedByUserId == _identity.UserId).ToList();

            if (!string.IsNullOrWhiteSpace(userId))
            {
                query = query.Where(x => x.Id == userId).ToList();
            }

            if (!string.IsNullOrWhiteSpace(referenceId))
            {
                query = query.Where(x => x.ReferenceId == referenceId).ToList();
            }

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                query = query.Where(x => x.FirstName == firstName).ToList();
            }

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                query = query.Where(x => x.LastName == lastName).ToList();
            }

            if (!string.IsNullOrWhiteSpace(contactGroupId))
            {
                query = query.Where(x => x.CreatedByUserId == contactGroupId).ToList();
            }

            return query;
        }

        public async Task<ExcelImportResponse<Contact>> ImportContactAsync(IFormFile formFile)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return new ExcelImportResponse<Contact>
                {
                    IsSuccess = false,
                    Message = "File is empty"
                };
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return new ExcelImportResponse<Contact>
                {
                    IsSuccess = false,
                    Message = "File extension should be '.xlsx'"
                };
            }

            var list = new List<Contact>();
            var errorList = new List<string>();
            var errorMessage = string.Empty;
            var dataCount = 0;
            var successCount = 0;
            var errorCount = 0;

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    dataCount = rowCount-1;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var FirstName = worksheet.Cells[row, 1].Text == null || worksheet.Cells[row, 1].Text == "" ?
                                        "" :
                                        worksheet.Cells[row, 1].Value.ToString();
                        var LastName = worksheet.Cells[row, 2].Text == null || worksheet.Cells[row, 2].Text == "" ? 
                                       "" :
                                       worksheet.Cells[row, 2].Value.ToString();
                        var Email = worksheet.Cells[row, 3].Text == null || worksheet.Cells[row, 3].Text == "" ?
                                    "" :
                                    worksheet.Cells[row, 3].Value.ToString();
                        var PhoneNumber = worksheet.Cells[row, 4].Text == null || worksheet.Cells[row, 4].Text == "" ?
                                          "" :
                                          worksheet.Cells[row, 4].Value.ToString();
                        var Notes = worksheet.Cells[row, 5].Text == null || worksheet.Cells[row, 5].Text == "" ?
                                    "" :
                                    worksheet.Cells[row, 5].Value.ToString();

                        //Check if data is valid
                        if (string.IsNullOrEmpty(PhoneNumber))
                        {
                            errorMessage = $"Error at row {row} : No phone number found";
                            errorList.Add(errorMessage);
                            continue;
                        }

                        var userPhoneNumber = _db.Contact.Where(x => x.PhoneNumber == PhoneNumber.Trim() && x.CreatedByUserId == _identity.UserId).ToList();
                        if (userPhoneNumber.Count != 0)
                        {
                            errorMessage = $"Error at row {row} : Phone number already exist";
                            errorList.Add(errorMessage);
                            continue;
                        }

                        var contact = new Contact
                        {
                            FirstName = FirstName != "" ? FirstName : PhoneNumber.Trim(),
                            LastName = LastName,
                            Email = Email.Trim(),
                            PhoneNumber = PhoneNumber.Trim(),
                            Notes = Notes
                        };

                        await _db.Contact.AddAsync(contact);
                        successCount++;
                    }

                    try
                    {
                        await _db.SaveChangesAsync(_identity.UserId);
                    }
                    catch (Exception e)
                    {
                        return new ExcelImportResponse<Contact>
                        {
                            Message = e.Message.ToString() + " " + e.InnerException.ToString(),
                            IsSuccess = false
                        };
                    }


                }
            }

            errorCount = errorList.Count();

            if(successCount > 0)
            {
                return new ExcelImportResponse<Contact>
                {
                    IsSuccess = true,
                    Message = $"{successCount} contact(s) out of {dataCount} imported successfully with {errorCount} errors",
                    Error = errorList
                };
            }
            else if(dataCount == 0)
            {
                return new ExcelImportResponse<Contact>
                {
                    IsSuccess = false,
                    Message = "No data found in the spreadsheet",
                    Error = errorList
                };
            }
            else
            {
                return new ExcelImportResponse<Contact>
                {
                    IsSuccess = false,
                    Message = $"{successCount} contact(s) out of {dataCount} imported successfully with {errorCount} errors",
                    Error = errorList
                };
            }

            
        }
    }
}
