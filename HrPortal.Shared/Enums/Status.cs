using System;
using System.ComponentModel;

namespace HrPortal.Shared.Enums
{
	public enum Status
	{
        [Description("啟用")]
        Enabled = 1,
        [Description("停用")]
        Disabled = 0
    }
}

