// =================================================
// File:
// SignalR.POC/SignalR.POC.RatesGainCapital/IGainCapitalRatesParser.cs
// 
// Last updated:
// 2013-08-18 4:07 PM
// =================================================

#region Usings

using System.Collections.Generic;

using SignalR.POC.Library.Models;

#endregion

namespace SignalR.POC.RatesGainCapital.Abstraction
{
	public interface IGainCapitalRatesParser
	{
		List<CurrencyPair> ParseToCurrencyPair(string message);
	}
}
