// =================================================
// File:
// SignalR.POC/SignalR.POC.ConsoleClient/Program.cs
// 
// Last updated:
// 2013-08-18 4:20 PM
// =================================================

#region Usings

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;

using SignalR.POC.Library.Models;

#endregion

namespace SignalR.POC.ConsoleClient
{
	public class Program
	{
		public static void Main()
		{
			RunAsync().Wait();
			Console.ReadKey();
		}

		private static async Task RunAsync()
		{
			TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

			var traceWriter = InitTraceWriter();

			var hubConnection = InitHubConnection(traceWriter);

			// Create hub proxy
			var hubProxy = hubConnection.CreateHubProxy("FXCMRatesHub");

			// Register methods
			hubProxy.On<IEnumerable<CurrencyPair>>("GetFirstQuotes", Console.WriteLine);

			hubProxy.On<CurrencyPair>("UpdateMarketPrice",
									  param => Console.WriteLine("Pair {0} - Bid {1}, Ask {2}", param.PairName, param.Bid, param.Ask));

			// Init parameters
			hubProxy["customerId"] = 100;

			try
			{
				await hubConnection.Start();
				Console.WriteLine("Success! Connected with client connection id {0}", hubConnection.ConnectionId);

				Thread.Sleep(5000);

				// Invoke methods
				var quotes = await hubProxy.Invoke<IEnumerable<CurrencyPair>>("GetFirstQuotes");
				foreach (var q in quotes)
				{
					Console.WriteLine("Pair {0} - Bid {1}, Ask {2}", q.PairName, q.Bid, q.Ask);
				}

				Console.WriteLine("Press any key to start quoting");
				Console.ReadKey();

				await hubProxy.Invoke("StartQuoting");
			}
			catch (Exception e)
			{
				Console.WriteLine("Failed to start: {0}", e.Message);
			}

			Console.ReadKey();
		}

		private static HubConnection InitHubConnection(TextWriter traceWriter)
		{
			var hubConnection = new HubConnection(ConfigurationManager.AppSettings["ServerUrl"]);

			hubConnection.TraceWriter = traceWriter;
			hubConnection.TraceLevel = TraceLevels.All;
			hubConnection.Closed += () => Console.WriteLine("[Closed]");
			hubConnection.ConnectionSlow += () => Console.WriteLine("[ConnectionSlow]");
			hubConnection.Error += error => Console.WriteLine("[Error] {0}", error.ToString());
			hubConnection.Received += (payload) => Console.WriteLine("[Received] {0}", payload);
			hubConnection.Reconnected += () => Console.WriteLine("[Reconnected]");
			hubConnection.Reconnecting += () => Console.WriteLine("[Reconnecting]");
			hubConnection.StateChanged += change => Console.WriteLine("[StateChanged] {0} {1}", change.OldState, change.NewState);
			hubConnection.TransportConnectTimeout = new TimeSpan(0, 0, 10);

			return hubConnection;
		}

		private static StreamWriter InitTraceWriter()
		{
			var traceWriter = new StreamWriter(@"C:\HubTracer.txt") {AutoFlush = true};
			return traceWriter;
		}

		private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
		{
			Console.WriteLine(e.Exception);
			throw e.Exception;
		}
	}
}
