// =================================================
// File:
// SignalR.POC/SignalR.POC.ConsoleServer/GainCapitalRatesProvider.cs
// 
// Last updated:
// 2013-08-18 4:11 PM
// =================================================

#region Usings

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;

using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Practices.Unity;

using SignalR.POC.ConsoleServer.DataProviders.Abstraction;
using SignalR.POC.ConsoleServer.Hubs;
using SignalR.POC.ConsoleServer.Resolver;
using SignalR.POC.Library.Models;
using SignalR.POC.Library.Synchronization;
using SignalR.POC.Logging.Abstraction;
using SignalR.POC.RatesGainCapital.Abstraction;
using SignalR.POC.RatesSpread.Abstraction;

#endregion

namespace SignalR.POC.ConsoleServer.DataProviders
{
	public class GainCapitalRatesProvider : IRatesProvider
	{
		private static ILoggerWrapper _wrapper;

		private static readonly Lazy<GainCapitalRatesProvider> Instance =
			new Lazy<GainCapitalRatesProvider>(
				() => new GainCapitalRatesProvider(GlobalHost.ConnectionManager.GetHubContext<GainCapitalRatesHub>().Clients));

		private readonly ICustomerInfoProvider _customerInfo;
		private readonly IGainCapitalRatesManager _gainCapitalRatesService;
		private readonly IRatesSpreadService _spreadProvider;

		private readonly Dictionary<string, Timer> _timers;
		private readonly TimeSpan _updateInterval;

		private GainCapitalRatesProvider(IHubConnectionContext clients)
		{
			_wrapper = DependencyFactory.Container.Resolve<ILoggerWrapper>();

			_gainCapitalRatesService = DependencyFactory.Container.Resolve<IGainCapitalRatesManager>();
			_gainCapitalRatesService.StartProcessing();

			_customerInfo = DependencyFactory.Container.Resolve<ICustomerInfoProvider>();
			_spreadProvider = DependencyFactory.Container.Resolve<IRatesSpreadService>();

			_updateInterval = TimeSpan.FromMilliseconds(Int32.Parse(ConfigurationManager.AppSettings["UpdateInterval"]));

			_timers = new Dictionary<string, Timer>();

			Clients = clients;
		}

		public static GainCapitalRatesProvider CurrentInstance
		{
			get
			{
				return Instance.Value;
			}
		}

		private IHubConnectionContext Clients { get; set; }

		public IEnumerable<CurrencyPair> GetFirstQuotes(int customerId)
		{
			var currencyPairs = new ConcurrentDictionary<string, CurrencyPair>();
			var pairNames = _customerInfo.GetListOfPairNames(customerId);

			// Wait until all quotes will be loaded
			while (currencyPairs.Count < pairNames.Count)
			{
				try
				{
					foreach (var p in pairNames)
					{
						var r = _gainCapitalRatesService.GetRate(p);

						if (r != null)
						{
							currencyPairs.AddOrUpdate(r.PairName, r, (key, oldValue) => r);
						}
					}
				}
				catch (Exception e)
				{
					_wrapper.Log.Error(e.Message);
					Console.WriteLine(e.Message);
				}
			}

			return currencyPairs.Values.ToList();
		}

		public void StartQuoting(string connectionId, int customerId)
		{
			if (!_gainCapitalRatesService.IsRunning)
			{
				_gainCapitalRatesService.StartProcessing();
			}

			var parameters = new QuotingParameters {ConnectionId = connectionId, CustomerId = customerId};

			var timer = new Timer(UpdateQuotes, parameters, _updateInterval, _updateInterval);
			_timers.Add(connectionId, timer);

			BroadcastMarketOpened(connectionId);
		}

		public void StopQuoting(string connectionId)
		{
			_timers.Remove(connectionId);
			GC.Collect();

			if (_gainCapitalRatesService.IsRunning)
			{
				_gainCapitalRatesService.StopProcessing();
			}

			BroadcastStatusMessage(connectionId, "Stopped");
			BroadcastMarketClosed(connectionId);
		}

		#region Private Methods

		private void UpdateQuotes(object state)
		{
			var parameters = (QuotingParameters)state;

			if (Sync.ConsolePauseEvent.WaitOne(0, false))
			{
				BroadcastStatusMessage(parameters.ConnectionId, "Paused");
			}
			else
			{
				BroadcastStatusMessage(parameters.ConnectionId, "Processing");

				try
				{
					var pairNames = _customerInfo.GetListOfPairNames(parameters.CustomerId);

					foreach (var p in pairNames)
					{
						var r = _gainCapitalRatesService.GetRate(p);

						if (r != null)
						{
							var spread = _spreadProvider.GetSpread(r.PairName, parameters.CustomerId);

							var cp = new CurrencyPair();

							cp.PairName = r.PairName;
							cp.Bid = r.Bid - spread.SpreadBid;
							cp.Ask = r.Ask - spread.SpreadAsk;
							cp.SpreadBid = spread.SpreadBid;
							cp.SpreadAsk = spread.SpreadAsk;

							BroadcastMarketPrice(cp, parameters.ConnectionId);
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

		private void BroadcastMarketPrice(CurrencyPair pair, string connectionId)
		{
			Clients.Client(connectionId).UpdateMarketPrice(pair);
		}

		private void BroadcastMarketOpened(string connectionId)
		{
			Clients.Client(connectionId).MarketOpened();
		}

		private void BroadcastMarketClosed(string connectionId)
		{
			Clients.Client(connectionId).MarketClosed();
		}

		private void BroadcastStatusMessage(string connectionId, string message)
		{
			Clients.Client(connectionId).NotifyUser(message);
		}

		#endregion Private Methods
	}
}
