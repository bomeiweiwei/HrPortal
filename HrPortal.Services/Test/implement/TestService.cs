using System;
using HrPortal.Models;
using HrPortal.Shared.Enums;

namespace HrPortal.Services.Test.implement
{
	public class TestService : BaseService, ITestService
	{
        public async Task<ResponseBase<bool>> GetConnectResult()
        {
            var result = new ResponseBase<bool>
            {
                Data = false
            };

            using (var context = base.MainDB(ConnectionMode.Slave))
            {
                result.Data = context.Database.CanConnect();
            }

            return result;
        }
    }
}

