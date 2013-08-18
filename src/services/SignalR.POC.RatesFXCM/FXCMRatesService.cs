// =================================================
// File:
// SignalR.POC/SignalR.POC.RatesFXCM/FXCMRatesService.cs
// 
// Last updated:
// 2013-08-18 4:07 PM
// =================================================

#region Usings

// =================================================
// File: 
// SignalR.POC.RatesFXCM/FXCMRatesService.cs
// 
// Created:
// 2013-08-06 10:43 AM
// 
// Last updated:
// 2013-08-06 4:06 PM
// =================================================

using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

using SignalR.POC.Library.Models;
using SignalR.POC.Logging.Abstraction;
using SignalR.POC.RatesFXCM.Abstraction;

#endregion

namespace SignalR.POC.RatesFXCM
{
	public class FXCMRatesService : IFXCMRatesService
	{
		private readonly CancellationTokenSource _cts;
		private readonly TaskFactory _factory;

		private readonly IFXCMRatesParser _parser;
		private readonly ILoggerWrapper _wrapper;

		private readonly string _url;

		public ConcurrentDictionary<string, CurrencyPair> Rates = new ConcurrentDictionary<string, CurrencyPair>();

		public FXCMRatesService(ILoggerWrapper wrapper, IFXCMRatesParser parser)
		{
			_wrapper = wrapper;
			_parser = parser;

			_url = ConfigurationManager.AppSettings["FXCMRatesUrl"];

			_cts = new CancellationTokenSource();
			_factory = new TaskFactory(_cts.Token);

			StartProcessing();
		}

		public bool IsRunning { get; set; }

		public CurrencyPair GetRate(string currencyPair)
		{
			CurrencyPair rate;
			Rates.TryGetValue(currencyPair, out rate);
			return rate;
		}

		public void StartProcessing()
		{
			IsRunning = true;
			_factory.StartNew(StartProcessingRatesAsync);
		}

		public void StopProcessing()
		{
			IsRunning = false;
			_cts.Cancel();
		}

		private void StartProcessingRatesAsync()
		{
			try
			{
				while (!_cts.IsCancellationRequested)
				{
					var doc = new XmlDocument();
					doc.Load(_url);

					var rates = _parser.ParseToCurrencyPair(doc);

					foreach (var r in rates)
					{
						var pair = r;
						Rates.AddOrUpdate(r.PairName, r, (key, oldValue) => pair);
					}
				}
			}
			catch (Exception e)
			{
				_wrapper.Log.Error(e.Message);
			}
		}
	}
}
