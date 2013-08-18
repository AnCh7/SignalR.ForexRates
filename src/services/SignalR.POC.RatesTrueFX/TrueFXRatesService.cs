// =================================================
// File:
// SignalR.POC/SignalR.POC.RatesTrueFX/TrueFXRatesService.cs
// 
// Last updated:
// 2013-08-18 5:10 PM
// =================================================

#region Usings

using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using SignalR.POC.Library.Models;
using SignalR.POC.Logging.Abstraction;
using SignalR.POC.RatesTrueFX.Abstraction;

#endregion

namespace SignalR.POC.RatesTrueFX
{
	public class TrueFXRatesService : ITrueFXRatesService
	{
		private readonly CancellationTokenSource _cts;
		private readonly TaskFactory _factory;

		private readonly ITrueFXRatesParser _parser;

		private readonly string _url;
		private readonly ILoggerWrapper _wrapper;

		public ConcurrentDictionary<string, CurrencyPair> Rates = new ConcurrentDictionary<string, CurrencyPair>();

		public TrueFXRatesService(ILoggerWrapper wrapper, ITrueFXRatesParser parser)
		{
			_wrapper = wrapper;
			_parser = parser;

			_url = ConfigurationManager.AppSettings["TrueFXRatesUrl"];

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
					var webClient = new WebClient();
					var page = webClient.DownloadString(_url);

					var rates = _parser.ParseToCurrencyPair(page);

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
