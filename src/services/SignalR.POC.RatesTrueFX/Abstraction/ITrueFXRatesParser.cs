// =================================================
// File:
// SignalR.POC/SignalR.POC.RatesTrueFX/ITrueFXRatesParser.cs
// 
// Last updated:
// 2013-08-18 4:06 PM
// =================================================

#region Usings

using System.Collections.Generic;

using SignalR.POC.Library.Models;

#endregion

namespace SignalR.POC.RatesTrueFX.Abstraction
{
	public interface ITrueFXRatesParser
	{
		List<CurrencyPair> ParseToCurrencyPair(string message);
	}
}
