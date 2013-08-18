// =================================================
// File:
// SignalR.POC/SignalR.POC.ConsoleServer/RatesSpreadProvider.cs
// 
// Last updated:
// 2013-08-18 4:11 PM
// =================================================

#region Usings

using System;

using SignalR.POC.ConsoleServer.DataProviders.Abstraction;
using SignalR.POC.Library.Models;
using SignalR.POC.Logging.Abstraction;
using SignalR.POC.RatesSpread.Abstraction;

#endregion

namespace SignalR.POC.ConsoleServer.DataProviders
{
	public class RatesSpreadProvider : IRatesSpreadProvider
	{
		private readonly IRatesSpreadService _provider;
		private readonly ILoggerWrapper _wrapper;

		public RatesSpreadProvider(ILoggerWrapper wrapper, IRatesSpreadService provider)
		{
			_wrapper = wrapper;
			_provider = provider;
		}

		public Spread GetSpread(string pairName, int customerId)
		{
			var spread = new Spread();

			try
			{
				spread = _provider.GetSpread(pairName, customerId);

				if (spread == null)
				{
					spread = new Spread {SpreadAsk = Decimal.Zero, SpreadBid = Decimal.Zero};
				}
			}
			catch (Exception e)
			{
				_wrapper.Log.Error(e.Message);
				Console.WriteLine(e.Message);
			}

			return spread;
		}
	}
}
