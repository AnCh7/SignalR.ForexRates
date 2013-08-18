// =================================================
// File:
// SignalR.POC/SignalR.POC.WinFormsClient/DependencyFactory.cs
// 
// Last updated:
// 2013-08-18 4:13 PM
// =================================================

#region Usings

using Microsoft.Practices.Unity;

using SignalR.POC.Authentication;
using SignalR.POC.Authentication.Abstraction;
using SignalR.POC.CustomerInfo;
using SignalR.POC.CustomerInfo.Abstraction;
using SignalR.POC.Logging;
using SignalR.POC.Logging.Abstraction;

#endregion

namespace SignalR.POC.WinFormsClient.Resolver
{
	/// <summary> Simple wrapper for unity resolution. </summary>
	public class DependencyFactory
	{
		/// <summary>
		/// Static constructor for DependencyFactory which will
		/// initialize the unity container.
		/// </summary>
		static DependencyFactory()
		{
			Container = new UnityContainer();

			Container.RegisterType<ILoggerWrapper, LoggerWrapper>(new ContainerControlledLifetimeManager());
			var logger = Container.Resolve<ILoggerWrapper>(new ParameterOverride("currentClassName", "SignalR.POC.WebClient"));
			Container.RegisterInstance(logger);

			Container.RegisterType<IAuthenticationService, AuthenticationService>(new ContainerControlledLifetimeManager());
			Container.RegisterType<ICustomerService, CustomerInfoService>(new ContainerControlledLifetimeManager());
		}

		/// <summary>
		/// Public reference to the unity container which will
		/// allow the ability to register instances or take other
		/// actions on the container.
		/// </summary>
		public static IUnityContainer Container { get; private set; }
	}
}
