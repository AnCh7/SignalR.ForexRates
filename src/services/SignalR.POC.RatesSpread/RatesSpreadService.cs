// =================================================
// File:
// SignalR.POC/SignalR.POC.RatesSpread/RatesSpreadService.cs
// 
// Last updated:
// 2013-08-18 4:06 PM
// =================================================

#region Usings

using System;

using SignalR.POC.Library.Models;
using SignalR.POC.Logging.Abstraction;
using SignalR.POC.RatesSpread.Abstraction;

#endregion

namespace SignalR.POC.RatesSpread
{
	// Implement obtaining spread for Bid and Ask in this class
	public class RatesSpreadService : IRatesSpreadService
	{
		private readonly ILoggerWrapper _wrapper;

		public RatesSpreadService(ILoggerWrapper wrapper)
		{
			_wrapper = wrapper;
		}

		public Spread GetSpread(string currencyPair, int customerCode)
		{
			var spr = new Spread();

			try
			{
				spr.SpreadAsk = new decimal(0.0003);
				spr.SpreadBid = new decimal(0.0003);
			}
			catch (Exception ex)
			{
				_wrapper.Log.Error(ex.Message);
				Console.WriteLine(ex.Message);
			}

			return spr;
		}
	}
}
