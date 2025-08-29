using System;
using System.Net;
using HrPortal.EF.Entities;
using HrPortal.Models;
using HrPortal.Models.Account;
using HrPortal.Models.Extensions;
using HrPortal.Shared.Enums;
using Microsoft.EntityFrameworkCore;


namespace HrPortal.Services.Identity.implement
{
	public class AccountService : BaseService, IAccountService
	{
        public async Task<ResponseBase<Account>> FindAccountByUserName(string userName)
        {
            var result = new ResponseBase<Account>()
            {
                Data = new Account()
            };
            using (var context = base.MainDB(ConnectionMode.Slave))
            {
                var query = await context.Accounts.FirstOrDefaultAsync(m => m.UserName == userName);
                if (query == null)
                {
                    return result.SetResponse(ReturnCode.DataNotExisted);
                }

                result.Data = query;
            }
            return result;
        }

        public async Task<ResponseBase<UserData>> VerifyUser(Guid accountId)
        {
            var result = new ResponseBase<UserData>()
            {
                Data = new UserData()
            };

            using (var context = base.MainDB(ConnectionMode.Slave))
            {
                var query = await (from a in context.Accounts
                                   join person in context.People on a.PersonId equals person.PersonId 
                                   where
                                    a.AccountId == accountId &&
                                    a.Status == (byte)Status.Enabled &&
                                    a.IsDeleted == false &&
                                    person.IsActive == true &&
                                    person.IsDeleted == false
                                   select new UserData
                                   {
                                       AccountId = a.AccountId,
                                       UserName = a.UserName,
                                       FullName = person.FullName
                                   }).FirstOrDefaultAsync();
                if (query == null)
                {
                    return result.SetResponse(ReturnCode.DataNotExisted);
                }

                result.Data = query;
            }

            return result;
        }
    }
}

