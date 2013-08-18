// =================================================
// File:
// SignalR.POC/SignalR.POC.CustomerInfo/ICustomerService.cs
// 
// Last updated:
// 2013-08-18 4:09 PM
// =================================================

#region Usings

using System.Collections.Generic;

#endregion

namespace SignalR.POC.CustomerInfo.Abstraction
{
	public interface ICustomerService
	{
		int GetCustomerId(string login, string password);

		List<string> GetAvailableCurrencyPairs(int customerId);
	}
}
