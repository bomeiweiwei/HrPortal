using System;
using HrPortal.Shared.Enums;
using HrPortal.Shared.Extensions;

namespace HrPortal.Models.Extensions
{
	public static class ResponseBaseExtension
	{
		public static ResponseBase<T> SetResponse<T>(this ResponseBase<T> response, ReturnCode returnCode)
		{
			response.StatusCode = (long)returnCode;
			response.Message=returnCode.GetDescription();
			return response;
        }
	}
}

