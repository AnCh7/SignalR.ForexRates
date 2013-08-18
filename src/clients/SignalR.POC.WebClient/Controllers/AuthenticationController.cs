// =================================================
// File:
// SignalR.POC/SignalR.POC.WebClient/AuthenticationController.cs
// 
// Last updated:
// 2013-08-18 4:18 PM
// =================================================

#region Usings

using System.Web.Mvc;
using System.Web.Routing;

using Microsoft.Practices.Unity;

using SignalR.POC.Authentication.Abstraction;
using SignalR.POC.CustomerInfo.Abstraction;
using SignalR.POC.WebClient.Models;
using SignalR.POC.WebClient.Resolver;

#endregion

namespace SignalR.POC.WebClient.Controllers
{
	public class AuthenticationController : Controller
	{
		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Index(User user)
		{
			var customerInfo = DependencyFactory.Container.Resolve<ICustomerService>();
			var authenticationService = DependencyFactory.Container.Resolve<IAuthenticationService>();

			var result = authenticationService.AuthenticateUser(user.Login, user.Password);

			if (result)
			{
				var customerId = customerInfo.GetCustomerId(user.Login, user.Password);
				return RedirectToAction("Index",
										new RouteValueDictionary(new {controller = "Home", action = "Index", CustomerId = customerId}));
			}
			else
			{
				ModelState.AddModelError(string.Empty, "Invalid credentials!");
			}

			return View(user);
		}
	}
}
