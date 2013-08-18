// =================================================
// File:
// SignalR.POC/SignalR.POC.ConsoleServer/Program.cs
// 
// Last updated:
// 2013-08-18 4:10 PM
// =================================================

#region Usings

using System;
using System.Configuration;

using Microsoft.Owin.Hosting;

using SignalR.POC.ConsoleServer.Configuration;
using SignalR.POC.Library.Synchronization;

#endregion

namespace SignalR.POC.ConsoleServer
{
	internal class Program
	{
		private static void Main()
		{
			var options = new StartOptions();
			options.Urls.Add(ConfigurationManager.AppSettings["ServerUrl"]);

			using (WebApp.Start<Startup>(options))
			{
				Console.WriteLine("Server running on {0}", options.Urls[0]);

				do
				{
					var readLine = Console.ReadLine();

					if (readLine != null)
					{
						if (readLine.ToLower() == "start")
						{
							Sync.ConsolePauseEvent.Reset();
						}
						if (readLine.ToLower() == "pause")
						{
							Sync.ConsolePauseEvent.Set();
						}
					}
				} while (true);
			}
		}
	}
}
