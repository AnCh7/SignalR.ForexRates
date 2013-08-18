// =================================================
// File:
// SignalR.POC/SignalR.POC.RatesGainCapital/IGainCapitalRatesService.cs
// 
// Last updated:
// 2013-08-18 4:07 PM
// =================================================

namespace SignalR.POC.RatesGainCapital.Abstraction
{
	public interface IGainCapitalRatesService
	{
		void StartService();

		void StopService();

		string GetResponse();
	}
}
