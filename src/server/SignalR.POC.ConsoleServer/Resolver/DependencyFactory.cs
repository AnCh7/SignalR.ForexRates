// =================================================
// File:
// SignalR.POC/SignalR.POC.ConsoleServer/DependencyFactory.cs
// 
// Last updated:
// 2013-08-18 4:10 PM
// =================================================

#region Usings

using System;

using Microsoft.Practices.Unity;

using SignalR.POC.Authentication;
using SignalR.POC.Authentication.Abstraction;
using SignalR.POC.ConsoleServer.DataProviders;
using SignalR.POC.ConsoleServer.DataProviders.Abstraction;
using SignalR.POC.CustomerInfo;
using SignalR.POC.CustomerInfo.Abstraction;
using SignalR.POC.Logging;
using SignalR.POC.Logging.Abstraction;
using SignalR.POC.RatesFXCM;
using SignalR.POC.RatesFXCM.Abstraction;
using SignalR.POC.RatesGainCapital;
using SignalR.POC.RatesGainCapital.Abstraction;
using SignalR.POC.RatesSpread;
using SignalR.POC.RatesSpread.Abstraction;
using SignalR.POC.RatesTrueFX;
using SignalR.POC.RatesTrueFX.Abstraction;

#endregion

namespace SignalR.POC.ConsoleServer.Resolver
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

			try
			{
				Container.RegisterType<ILoggerWrapper, LoggerWrapper>(new ContainerControlledLifetimeManager());
				var logger = Container.Resolve<ILoggerWrapper>(new ParameterOverride("currentClassName", "SignalR.POC.WebClient"));
				Container.RegisterInstance(logger);

				Container.RegisterType<IRatesSpreadService, RatesSpreadService>(new ContainerControlledLifetimeManager());
				Container.RegisterType<ICustomerService, CustomerInfoService>(new ContainerControlledLifetimeManager());

				Container.RegisterType<IAuthenticationService, AuthenticationService>(new ContainerControlledLifetimeManager());

				Container.RegisterType<IFXCMRatesParser, FXCMRatesParser>(new ContainerControlledLifetimeManager());
				Container.RegisterType<IFXCMRatesService, FXCMRatesService>(new ContainerControlledLifetimeManager());

				Container.RegisterType<IGainCapitalRatesParser, GainCapitalRatesParser>(new ContainerControlledLifetimeManager());
				Container.RegisterType<IGainCapitalRatesService, GainCapitalRatesService>(new ContainerControlledLifetimeManager());
				Container.RegisterType<IGainCapitalRatesManager, GainCapitalRatesManager>(new ContainerControlledLifetimeManager());

				Container.RegisterType<ITrueFXRatesParser, TrueFXRatesParser>(new ContainerControlledLifetimeManager());
				Container.RegisterType<ITrueFXRatesService, TrueFXRatesService>(new ContainerControlledLifetimeManager());

				Container.RegisterType<IRatesSpreadProvider, RatesSpreadProvider>(new ContainerControlledLifetimeManager());
				Container.RegisterType<ICustomerInfoProvider, CustomerInfoProvider>(new ContainerControlledLifetimeManager());

				Container.RegisterType<IRatesProvider, GainCapitalRatesProvider>("Gain");
				Container.RegisterType<IRatesProvider, FXCMRatesProvider>("FXCM");
				Container.RegisterType<IRatesProvider, TrueFXRatesProvider>("TrueFX");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		/// <summary>
		/// Public reference to the unity container which will
		/// allow the ability to register instances or take other
		/// actions on the container.
		/// </summary>
		public static IUnityContainer Container { get; private set; }
	}
}
