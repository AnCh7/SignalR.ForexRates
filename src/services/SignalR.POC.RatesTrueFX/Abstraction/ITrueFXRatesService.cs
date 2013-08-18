// =================================================
// File:
// SignalR.POC/SignalR.POC.RatesTrueFX/ITrueFXRatesService.cs
// 
// Last updated:
// 2013-08-18 4:06 PM
// =================================================

#region Usings

using SignalR.POC.Library.Models;

#endregion

namespace SignalR.POC.RatesTrueFX.Abstraction
{
	public interface ITrueFXRatesService
	{
		CurrencyPair GetRate(string currencyPair);

		void StartProcessing();

		void StopProcessing();

		bool IsRunning { get; set; }
	}
}
