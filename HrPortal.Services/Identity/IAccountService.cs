using System;
using HrPortal.EF.Entities;
using HrPortal.Models;
using HrPortal.Models.Account;

namespace HrPortal.Services.Identity
{
	public interface IAccountService
	{
        Task<ResponseBase<Account>> FindAccountByUserName(string userName);

        Task<ResponseBase<UserData>> VerifyUser(Guid accountId);
    }
}

