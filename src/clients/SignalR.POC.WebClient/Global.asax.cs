// =================================================
// File:
// SignalR.POC/SignalR.POC.WebClient/Global.asax.cs
// 
// Last updated:
// 2013-08-18 4:17 PM
// =================================================

#region Usings

using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using SignalR.POC.WebClient.App_Start;

#endregion

namespace SignalR.POC.WebClient
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}
	}
}
