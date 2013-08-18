// =================================================
// File:
// SignalR.POC/SignalR.POC.WebClient/RouteConfig.cs
// 
// Last updated:
// 2013-08-18 4:20 PM
// =================================================

#region Usings

using System.Web.Mvc;
using System.Web.Routing;

#endregion

namespace SignalR.POC.WebClient.App_Start
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute("Default",
							"{controller}/{action}/{id}",
							new {controller = "Authentication", action = "Index", id = UrlParameter.Optional});
		}
	}
}
