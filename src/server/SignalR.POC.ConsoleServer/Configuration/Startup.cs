// =================================================
// File:
// SignalR.POC/SignalR.POC.ConsoleServer/Startup.cs
// 
// Last updated:
// 2013-08-18 4:11 PM
// =================================================

#region Usings

using Microsoft.AspNet.SignalR;

using Owin;

#endregion

namespace SignalR.POC.ConsoleServer.Configuration
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			var config = new HubConfiguration {EnableCrossDomain = true, EnableDetailedErrors = true};
			app.MapHubs(config);
		}
	}
}
