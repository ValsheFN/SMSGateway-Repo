﻿using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json.Linq;
using SMSGateway.Server.Infrastructure;
using SMSGateway.Server.Models;
using SMSGateway.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace SMSGateway.Server.Services
{
    public interface ITopUpService
    {
        Task<OperationResponse<TopUp>> CreateAsync(TopUp model);
        List<TopUp> GetAllFiltered(string referenceId, string requester, string status,
                                          DateTime requestDateStart, DateTime requestDateEnd,
                                          DateTime grantDateStart, DateTime grantDateEnd,
                                          string grantedBy);
        Task<OperationResponse<TopUp>> UpdateTopUp(string referenceId, string action);
    }

    public class TopUpService : ITopUpService
    {
        private readonly ApplicationDBContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IdentityOption _identity;

        public TopUpService(ApplicationDBContext db, UserManager<ApplicationUser> userManager, IdentityOption identity)
        {
            _db = db;
            _userManager = userManager;
            _identity = identity;
        }
        public async Task<OperationResponse<TopUp>> CreateAsync(TopUp model)
        {
            var topUp = new TopUp
            {
                TopUpValue = model.TopUpValue,
                Requester = model.Requester,
                Status = "Pending",
                RequestDate = DateTime.Now
            };

            await _db.TopUps.AddAsync(topUp);
            await _db.SaveChangesAsync(_identity.UserId);

            model.Id = topUp.Id;

            return new OperationResponse<TopUp>
            {
                Message = "Top up created successfully!",
                IsSuccess = true,
                Data = model
            };
        }

        public List<TopUp> GetAllFiltered(string referenceId, string requester, string status, 
                                          DateTime requestDateStart, DateTime requestDateEnd,
                                          DateTime grantDateStart, DateTime grantDateEnd,
                                          string grantedBy)
        {
            var query = _db.TopUps.ToList();

            query = query.Where(x => x.CreatedByUserId == _identity.UserId).ToList();

            if (!string.IsNullOrWhiteSpace(referenceId))
            {
                query = query.Where(x => x.ReferenceId == referenceId).ToList();
            }

            if (!string.IsNullOrWhiteSpace(requester))
            {
                query = query.Where(x => x.Requester == requester).ToList();
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(x => x.Status == status).ToList();
            }

            if (requestDateStart != DateTime.MinValue)
            {
                query = query.Where(x => x.RequestDate >= requestDateStart).ToList();
            }

            if (requestDateEnd != DateTime.MinValue)
            {
                query = query.Where(x => x.RequestDate <= requestDateEnd).ToList();
            }

            if (grantDateStart != DateTime.MinValue)
            {
                query = query.Where(x => x.GrantDate >= grantDateStart).ToList();
            }

            if (grantDateEnd != DateTime.MinValue)
            {
                query = query.Where(x => x.GrantDate <= grantDateEnd).ToList();
            }

            return query;
        }

        public async Task<OperationResponse<TopUp>> UpdateTopUp(string referenceId, string action)
        {
            var topUpData = _db.TopUps.FirstOrDefault(x => x.ReferenceId == referenceId && x.CreatedByUserId == _identity.UserId);

            var topUpRequester = topUpData.Requester;
            if(topUpData == null)
            {
                return new OperationResponse<TopUp>
                {
                    Message = "Top up data is not found",
                    IsSuccess = false
                };
            }

            if(action == "Grant")
            {
                var userData = await _userManager.FindByIdAsync(topUpRequester);
                if (userData == null)
                {
                    return new OperationResponse<TopUp>
                    {
                        Message = "No such user found",
                        IsSuccess = false
                    };
                }

                var totalCreditValue = (topUpData.TopUpValue/userData.CostPerSms) + userData.SmsCredit;

                userData.SmsCredit = Convert.ToInt32(totalCreditValue);

                var result = await _userManager.UpdateAsync(userData);

                topUpData.Status = "Success";

                _db.TopUps.Update(topUpData);
                await _db.SaveChangesAsync(_identity.UserId);

                return new OperationResponse<TopUp>
                {
                    Message = "Top up granted successfully!",
                    IsSuccess = true,
                    Data = topUpData
                };
            }
            else
            {
                topUpData.Status = "Rejected";
                _db.TopUps.Update(topUpData);
                await _db.SaveChangesAsync(_identity.UserId);

                return new OperationResponse<TopUp>
                {
                    Message = "Top up rejected successfully!",
                    IsSuccess = true,
                    Data = topUpData
                };
            }


        }
    }
}
