// =================================================
// File:
// SignalR.POC/SignalR.POC.RatesGainCapital/GainCapitalRatesManager.cs
// 
// Last updated:
// 2013-08-18 4:07 PM
// =================================================

#region Usings

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

using SignalR.POC.Library.Models;
using SignalR.POC.Logging.Abstraction;
using SignalR.POC.RatesGainCapital.Abstraction;

#endregion

namespace SignalR.POC.RatesGainCapital
{
	public sealed class GainCapitalRatesManager : IGainCapitalRatesManager
	{
		private readonly CancellationTokenSource _cts;
		private readonly TaskFactory _factory;

		private readonly IGainCapitalRatesParser _gainCapitalRatesParser;
		private readonly IGainCapitalRatesService _gainCapitalRatesService;
		private readonly ILoggerWrapper _wrapper;

		public ConcurrentDictionary<string, CurrencyPair> Rates = new ConcurrentDictionary<string, CurrencyPair>();

		public GainCapitalRatesManager(ILoggerWrapper wrapper,
									   IGainCapitalRatesService gainCapitalRatesService,
									   IGainCapitalRatesParser gainCapitalRatesParser)
		{
			if (wrapper == null)
			{
				throw new ArgumentNullException("wrapper");
			}

			if (gainCapitalRatesService == null)
			{
				throw new ArgumentNullException("gainCapitalRatesService");
			}

			if (gainCapitalRatesParser == null)
			{
				throw new ArgumentNullException("gainCapitalRatesParser");
			}

			_wrapper = wrapper;
			_gainCapitalRatesParser = gainCapitalRatesParser;
			_gainCapitalRatesService = gainCapitalRatesService;

			_cts = new CancellationTokenSource();
			_factory = new TaskFactory(_cts.Token);
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
					var response = _gainCapitalRatesService.GetResponse();

					if (!string.IsNullOrEmpty(response))
					{
						var pairs = _gainCapitalRatesParser.ParseToCurrencyPair(response);

						foreach (var p in pairs)
						{
							var pair = p;
							Rates.AddOrUpdate(p.PairName, p, (key, oldValue) => pair);
						}
					}
				}
			}
			catch (Exception e)
			{
				_wrapper.Log.Error(e.Message);
				Console.WriteLine(e.Message);
			}
		}
	}
}
