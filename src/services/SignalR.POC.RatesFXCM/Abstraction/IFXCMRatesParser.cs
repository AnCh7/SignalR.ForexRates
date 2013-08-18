// =================================================
// File:
// SignalR.POC/SignalR.POC.RatesFXCM/IFXCMRatesParser.cs
// 
// Last updated:
// 2013-08-18 4:08 PM
// =================================================

#region Usings

using System.Collections.Generic;
using System.Xml;

using SignalR.POC.Library.Models;

#endregion

namespace SignalR.POC.RatesFXCM.Abstraction
{
	public interface IFXCMRatesParser
	{
		List<CurrencyPair> ParseToCurrencyPair(XmlDocument message);
	}
}
