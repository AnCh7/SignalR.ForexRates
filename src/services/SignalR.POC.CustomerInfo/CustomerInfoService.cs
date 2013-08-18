// =================================================
// File:
// SignalR.POC/SignalR.POC.CustomerInfo/CustomerInfoService.cs
// 
// Last updated:
// 2013-08-18 4:08 PM
// =================================================

#region Usings

using System;
using System.Collections.Generic;

using SignalR.POC.CustomerInfo.Abstraction;
using SignalR.POC.Logging.Abstraction;

#endregion

namespace SignalR.POC.CustomerInfo
{
	// Implement obtaining customer Id and list of currency pairs available for this customer in this class
	public class CustomerInfoService : ICustomerService
	{
		private readonly ILoggerWrapper _wrapper;

		public CustomerInfoService(ILoggerWrapper wrapper)
		{
			_wrapper = wrapper;
		}

		public int GetCustomerId(string login, string password)
		{
			var customerId = 0;
			try
			{
				customerId = 100;
			}
			catch (Exception e)
			{
				_wrapper.Log.Error(e.Message);
			}

			return customerId;
		}

		public List<string> GetAvailableCurrencyPairs(int customerId)
		{
			var result = new List<string>();
			try
			{
				lock (result)
				{
					result = new List<string>
					{
						"EUR/USD",
						"USD/JPY",
						"GBP/USD",
						"EUR/GBP",
						"USD/CHF",
						"EUR/JPY",
						"USD/CAD",
						"AUD/USD",
						"GBP/JPY"
					};
				}
			}
			catch (Exception e)
			{
				_wrapper.Log.Error(e.Message);
			}

			return result;
		}
	}
}
