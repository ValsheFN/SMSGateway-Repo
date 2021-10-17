﻿using Microsoft.AspNetCore.Identity;
using SMSGateway.Server.Models;
using SMSGateway.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public TopUpService(ApplicationDBContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
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
            await _db.SaveChangesAsync();

            model.Id = topUp.Id;

            return new OperationResponse<TopUp>
            {
                Message = "Contact created successfully!",
                IsSuccess = true,
                Data = model
            };
        }

        public List<TopUp> GetAllFiltered(string referenceId, string requester, string status, 
                                          DateTime requestDateStart, DateTime requestDateEnd,
                                          DateTime grantDateStart, DateTime grantDateEnd,
                                          string grantedBy)
        {
            return _db.TopUps.Where(x => x.ReferenceId == referenceId
                                && x.Requester == requester
                                && x.Status == status
                                && x.RequestDate >= requestDateStart
                                && x.RequestDate <= requestDateEnd
                                && x.GrantDate >= grantDateStart
                                && x.GrantDate <= grantDateEnd
                                && x.GrantedBy == grantedBy)
                             .ToList();
        }

        public async Task<OperationResponse<TopUp>> UpdateTopUp(string referenceId, string action)
        {
            var topUpData = _db.TopUps.FirstOrDefault(x => x.ReferenceId == referenceId);
            var topUpRequester = topUpData.Requester;
            if(topUpData == null)
            {
                return new OperationResponse<TopUp>
                {
                    Message = "No such reference Id found",
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

                var totalCreditValue = topUpData.TopUpValue + userData.CreditValue;

                var userDataUpdated = new ApplicationUser
                {
                    Id = userData.Id,
                    UserName = userData.UserName,
                    Email = userData.Email,
                    CreditValue = totalCreditValue
                };

                var result = await _userManager.UpdateAsync(userDataUpdated);

                var topUp = new TopUp
                {
                    ReferenceId = topUpData.ReferenceId,
                    TopUpValue = topUpData.TopUpValue,
                    Status = "Success",
                    Requester = topUpData.Requester,
                    RequestDate = topUpData.RequestDate
                };


                await _db.TopUps.AddAsync(topUp);
                await _db.SaveChangesAsync();

                return new OperationResponse<TopUp>
                {
                    Message = "Top up granted successfully!",
                    IsSuccess = true,
                    Data = topUp
                };
            }
            else
            {
                var topUp = new TopUp
                {
                    ReferenceId = topUpData.ReferenceId,
                    TopUpValue = topUpData.TopUpValue,
                    Status = "Rejected",
                    Requester = topUpData.Requester,
                    RequestDate = topUpData.RequestDate
                };


                await _db.TopUps.AddAsync(topUp);
                await _db.SaveChangesAsync();

                return new OperationResponse<TopUp>
                {
                    Message = "Top up rejected successfully!",
                    IsSuccess = true,
                    Data = topUp
                };
            }


        }
    }
}
