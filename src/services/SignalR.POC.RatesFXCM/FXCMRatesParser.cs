// =================================================
// File:
// SignalR.POC/SignalR.POC.RatesFXCM/FXCMRatesParser.cs
// 
// Last updated:
// 2013-08-18 4:08 PM
// =================================================

#region Usings

using System;
using System.Collections.Generic;
using System.Xml;

using SignalR.POC.Library.Models;
using SignalR.POC.Logging.Abstraction;
using SignalR.POC.RatesFXCM.Abstraction;

#endregion

namespace SignalR.POC.RatesFXCM
{
	public sealed class FXCMRatesParser : IFXCMRatesParser
	{
		private readonly ILoggerWrapper _wrapper;

		public FXCMRatesParser(ILoggerWrapper wrapper)
		{
			_wrapper = wrapper;
		}

		//<Rates>
		//<Rate Symbol="EURUSD">
		//<Bid>1.32803</Bid>
		//<Ask>1.32826</Ask>
		//<High>1.32974</High>
		//<Low>1.3245</Low>
		//<Direction>-1</Direction>
		//<Last>06:36:37</Last>
		//</Rate>
		public List<CurrencyPair> ParseToCurrencyPair(XmlDocument message)
		{
			var pairsList = new List<CurrencyPair>();

			try
			{
				var parentList = message.ChildNodes;
				foreach (XmlNode parentNode in parentList)
				{
					var childList = parentNode.ChildNodes;
					foreach (XmlNode childNode in childList)
					{
						var pair = new CurrencyPair();

						if (childNode.Attributes != null)
						{
							pair.PairName = (childNode.Attributes["Symbol"].Value);
						}

						var nodeBid = childNode["Bid"];
						if (nodeBid != null)
						{
							pair.Bid = Convert.ToDecimal(nodeBid.InnerText);
						}

						var nodeAsk = childNode["Ask"];
						if (nodeAsk != null)
						{
							pair.Ask = Convert.ToDecimal(nodeAsk.InnerText);
						}

						var nodeHigh = childNode["High"];
						if (nodeHigh != null)
						{
							var high = Convert.ToDecimal(nodeHigh.InnerText);
						}

						var nodeLow = childNode["Low"];
						if (nodeLow != null)
						{
							var low = Convert.ToDecimal(nodeLow.InnerText);
						}

						var nodeDir = childNode["Direction"];
						if (nodeDir != null)
						{
							var direction = Convert.ToInt32(nodeDir.InnerText);
						}

						var nodeLast = childNode["Last"];
						if (nodeLast != null)
						{
							var date = Convert.ToDateTime(nodeLast.InnerText);
						}

						pairsList.Add(pair);
					}
				}
			}
			catch (Exception ex)
			{
				_wrapper.Log.Error(ex.Message);
				Console.WriteLine(ex.Message);
			}

			return pairsList;
		}
	}
}
