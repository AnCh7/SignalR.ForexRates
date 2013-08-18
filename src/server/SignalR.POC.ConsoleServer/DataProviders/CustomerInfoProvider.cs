// =================================================
// File:
// SignalR.POC/SignalR.POC.ConsoleServer/CustomerInfoProvider.cs
// 
// Last updated:
// 2013-08-18 4:11 PM
// =================================================

#region Usings

using System;
using System.Collections.Generic;

using SignalR.POC.ConsoleServer.DataProviders.Abstraction;
using SignalR.POC.CustomerInfo.Abstraction;
using SignalR.POC.Logging.Abstraction;

#endregion

namespace SignalR.POC.ConsoleServer.DataProviders
{
	public class CustomerInfoProvider : ICustomerInfoProvider
	{
		private readonly ICustomerService _dataProvider;
		private readonly ILoggerWrapper _wrapper;

		public CustomerInfoProvider(ILoggerWrapper wrapper, ICustomerService dataProvider)
		{
			_wrapper = wrapper;
			_dataProvider = dataProvider;
		}

		public List<string> GetListOfPairNames(int customerId)
		{
			var pairNames = new List<string>();

			try
			{
				pairNames = _dataProvider.GetAvailableCurrencyPairs(customerId);
			}
			catch (Exception e)
			{
				_wrapper.Log.Error(e.Message);
				Console.WriteLine(e.Message);
			}

			return pairNames;
		}
	}
}
