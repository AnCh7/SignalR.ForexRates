// =================================================
// File:
// SignalR.POC/SignalR.POC.RatesSpread/IRatesSpreadService.cs
// 
// Last updated:
// 2013-08-18 4:07 PM
// =================================================

#region Usings

using SignalR.POC.Library.Models;

#endregion

namespace SignalR.POC.RatesSpread.Abstraction
{
	public interface IRatesSpreadService
	{
		Spread GetSpread(string currencyPair, int customerCode);
	}
}
