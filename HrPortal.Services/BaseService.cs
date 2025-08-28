using System;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using HrPortal.EF.Extensions;
using HrPortal.Shared.Enums;
using HrPortal.Shared.SysConfigs;
using HrPortal.EF.Data;

namespace HrPortal.Services
{
	public class BaseService
	{
		public BaseService()
		{
		}

        protected virtual PersonSystemContext MainDB([Optional] ConnectionMode connectionMode)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PersonSystemContext>();
            if (connectionMode == ConnectionMode.Master)
            {
                optionsBuilder.OptionsBuilderSetting(ConfigManager.ConnectionStrings.Master);
            }
            else
            {
                optionsBuilder.OptionsBuilderSetting(ConfigManager.ConnectionStrings.Slave);
            }
            return new PersonSystemContext(optionsBuilder.Options);
        }
    }
}

