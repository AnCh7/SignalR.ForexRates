// =================================================
// File:
// SignalR.POC/SignalR.POC.ConsoleServer/TrueFXRatesHub.cs
// 
// Last updated:
// 2013-08-18 4:11 PM
// =================================================

#region Usings

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

using SignalR.POC.ConsoleServer.DataProviders;
using SignalR.POC.Library.Models;

#endregion

namespace SignalR.POC.ConsoleServer.Hubs
{
	[HubName("TrueFXRatesHub")]
	public class TrueFXRatesHub : Hub
	{
		private readonly TrueFXRatesProvider _ratesProvider;

		public TrueFXRatesHub() : this(TrueFXRatesProvider.CurrentInstance)
		{}

		public TrueFXRatesHub(TrueFXRatesProvider ratesProvider)
		{
			_ratesProvider = ratesProvider;
		}

		public IEnumerable<CurrencyPair> GetFirstQuotes()
		{
			int customerId = int.Parse((Clients.Caller.customerId).ToString());
			return _ratesProvider.GetFirstQuotes(customerId);
		}

		public void StartQuoting()
		{
			int customerId = int.Parse((Clients.Caller.customerId).ToString());
			_ratesProvider.StartQuoting(Context.ConnectionId, customerId);
		}

		public void StopQuoting()
		{
			_ratesProvider.StopQuoting(Context.ConnectionId);
		}

		public override Task OnConnected()
		{
			Console.WriteLine("User {0} connected", Context.ConnectionId);
			return base.OnConnected();
		}

		public override Task OnDisconnected()
		{
			Console.WriteLine("User {0} disconnected", Context.ConnectionId);
			return base.OnDisconnected();
		}

		public override Task OnReconnected()
		{
			Console.WriteLine("User {0} reconnected", Context.ConnectionId);
			Clients.Client(Context.ConnectionId).StartQuoting();
			return base.OnReconnected();
		}
	}
}
