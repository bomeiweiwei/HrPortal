using System;
using HrPortal.Models;

namespace HrPortal.Services.Test
{
	public interface ITestService
	{
        Task<ResponseBase<bool>> GetConnectResult();
    }
}

