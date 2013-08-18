// =================================================
// File:
// SignalR.POC/SignalR.POC.WebClient/WebApiConfig.cs
// 
// Last updated:
// 2013-08-18 4:20 PM
// =================================================

#region Usings

using System.Web.Http;

#endregion

namespace SignalR.POC.WebClient.App_Start
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new {id = RouteParameter.Optional});
		}
	}
}
