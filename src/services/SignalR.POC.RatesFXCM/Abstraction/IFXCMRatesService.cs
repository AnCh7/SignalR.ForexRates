// =================================================
// File:
// SignalR.POC/SignalR.POC.RatesFXCM/IFXCMRatesService.cs
// 
// Last updated:
// 2013-08-18 4:08 PM
// =================================================

#region Usings

using SignalR.POC.Library.Models;

#endregion

namespace SignalR.POC.RatesFXCM.Abstraction
{
	public interface IFXCMRatesService
	{
		CurrencyPair GetRate(string currencyPair);

		void StartProcessing();

		void StopProcessing();

		bool IsRunning { get; set; }
	}
}
