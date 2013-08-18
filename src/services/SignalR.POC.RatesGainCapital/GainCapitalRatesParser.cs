// =================================================
// File:
// SignalR.POC/SignalR.POC.RatesGainCapital/GainCapitalRatesParser.cs
// 
// Last updated:
// 2013-08-18 4:07 PM
// =================================================

#region Usings

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using SignalR.POC.Library.Models;
using SignalR.POC.Logging.Abstraction;
using SignalR.POC.RatesGainCapital.Abstraction;

#endregion

namespace SignalR.POC.RatesGainCapital
{
	public class GainCapitalRatesParser : IGainCapitalRatesParser
	{
		private readonly ILoggerWrapper _wrapper;

		public GainCapitalRatesParser(ILoggerWrapper wrapper)
		{
			_wrapper = wrapper;
		}

		// S22\AUD/CAD\0.92425\0.92590\0.92930\0.92053\D\A\5\0.92333\$
		// SA\AUD/USD\Bid\Ask\High\Low\D\A\4\Close\$
		public List<CurrencyPair> ParseToCurrencyPair(string message)
		{
			var pairsList = new List<CurrencyPair>();

			if (message.IndexOf('S') == 0)
			{
				message = message.Remove(0, 1);
			}

			var m = Regex.Replace(message, @"\r\n?|\n", Environment.NewLine);
			var list = m.Split('$');

			foreach (var s in list)
			{
				if (!string.IsNullOrEmpty(s) &&
					!s.Contains(Environment.NewLine))
				{
					var p = s.Split('\\');

					if (p.Length == 11)
					{
						try
						{
							var pair = new CurrencyPair();

							var pairId = p[0];
							pair.PairName = p[1];
							pair.Ask = Convert.ToDecimal(p[2]);
							pair.Bid = Convert.ToDecimal(p[3]);
							var high = Convert.ToDecimal(p[4]);
							var low = Convert.ToDecimal(p[5]);
							var close = Convert.ToDecimal(p[9]);

							pairsList.Add(pair);
						}
						catch (Exception ex)
						{
							_wrapper.Log.Error(ex.Message);
						}
					}
					else
					{
						_wrapper.Log.Error("Partial data from data feed");
						Console.WriteLine("Partial data from data feed");
					}
				}
			}

			return pairsList;
		}
	}
}
