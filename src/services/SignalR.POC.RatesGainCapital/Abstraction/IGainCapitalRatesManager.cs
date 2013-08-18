// =================================================
// File:
// SignalR.POC/SignalR.POC.RatesGainCapital/IGainCapitalRatesManager.cs
// 
// Last updated:
// 2013-08-18 4:07 PM
// =================================================

#region Usings

using SignalR.POC.Library.Models;

#endregion

namespace SignalR.POC.RatesGainCapital.Abstraction
{
	public interface IGainCapitalRatesManager
	{
		CurrencyPair GetRate(string currencyPair);

		void StartProcessing();

		void StopProcessing();

		bool IsRunning { get; set; }
	}
}
