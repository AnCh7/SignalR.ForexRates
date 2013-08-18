// =================================================
// File:
// SignalR.POC/SignalR.POC.WebClient/HomeController.cs
// 
// Last updated:
// 2013-08-18 4:19 PM
// =================================================

#region Usings

using System.Web.Mvc;
using System.Web.Routing;

#endregion

namespace SignalR.POC.WebClient.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index(int customerId)
		{
			TempData["customerId"] = customerId;
			return View();
		}

		public ActionResult GainCapitalRates()
		{
			var customerId = TempData["customerId"];
			return RedirectToAction("Index",
									new RouteValueDictionary(new {controller = "GainCapitalRates", action = "Index", CustomerId = customerId}));
		}

		public ActionResult FXCMRates()
		{
			var customerId = TempData["customerId"];
			return RedirectToAction("Index",
									new RouteValueDictionary(new {controller = "FXCMRates", action = "Index", CustomerId = customerId}));
		}

		public ActionResult TrueFXRates()
		{
			var customerId = TempData["customerId"];
			return RedirectToAction("Index",
									new RouteValueDictionary(new {controller = "TrueFXRates", action = "Index", CustomerId = customerId}));
		}
	}
}
