// =================================================
// File:
// SignalR.POC/SignalR.POC.RatesTrueFX/TrueFXRatesParser.cs
// 
// Last updated:
// 2013-08-18 4:06 PM
// =================================================

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;

using HtmlAgilityPack;

using SignalR.POC.Library.Models;
using SignalR.POC.Logging.Abstraction;
using SignalR.POC.RatesTrueFX.Abstraction;

#endregion

namespace SignalR.POC.RatesTrueFX
{
	public sealed class TrueFXRatesParser : ITrueFXRatesParser
	{
		private readonly ILoggerWrapper _wrapper;

		public TrueFXRatesParser(ILoggerWrapper wrapper)
		{
			_wrapper = wrapper;
		}

		//<table>
		//<tr>
		//<td>EUR/USD</td>
		//<td>1376052534888</td>
		//<td>1.33</td>
		//<td>702</td>
		//<td>1.33</td>
		//<td>710</td>
		//<td>1.33654</td>
		//<td>1.33909</td>
		//<td>1.33810</td>
		//</tr>
		public List<CurrencyPair> ParseToCurrencyPair(string message)
		{
			var pairsList = new List<CurrencyPair>();

			try
			{
				var doc = new HtmlDocument();
				doc.LoadHtml(message);

				var table =
					doc.DocumentNode.SelectSingleNode("//table")
					   .Descendants("tr")
					   .Where(tr => tr.Elements("td").Count() > 1)
					   .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
					   .ToList();

				foreach (var t in table)
				{
					var pair = new CurrencyPair();

					pair.PairName = t[0];
					pair.Bid = Convert.ToDecimal(t[2] + t[3]);
					pair.Ask = Convert.ToDecimal(t[4] + t[5]);

					var high = Convert.ToDecimal(t[6]);
					var low = Convert.ToDecimal(t[7]);
					var close = Convert.ToDecimal(t[8]);

					pairsList.Add(pair);
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
