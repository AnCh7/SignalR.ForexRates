// =================================================
// File:
// SignalR.POC/SignalR.POC.WebClient/FilterConfig.cs
// 
// Last updated:
// 2013-08-18 4:20 PM
// =================================================

#region Usings

using System.Web.Mvc;

#endregion

namespace SignalR.POC.WebClient.App_Start
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}
