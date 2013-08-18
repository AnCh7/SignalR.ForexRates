// =================================================
// File:
// SignalR.POC/SignalR.POC.Tests/ServicesTests.cs
// 
// Last updated:
// 2013-08-18 4:04 PM
// =================================================

#region Usings

using NUnit.Framework;

using Rhino.Mocks;

using SignalR.POC.Logging.Abstraction;
using SignalR.POC.RatesFXCM;
using SignalR.POC.RatesGainCapital;
using SignalR.POC.RatesSpread;
using SignalR.POC.RatesTrueFX;

#endregion

namespace SignalR.POC.Tests
{
	[TestFixture]
	public class ServicesTests
	{
		private readonly ILoggerWrapper _logger;

		public ServicesTests()
		{
			_logger = MockRepository.GenerateStub<ILoggerWrapper>();
		}

		[Test]
		public void RunFXCMRatesService()
		{
			var provider = new FXCMRatesService(_logger, new FXCMRatesParser(_logger));

			while (true)
			{}
		}

		[Test]
		public void RunGainCapitalRatesService()
		{
			var s = new GainCapitalRatesService(_logger);
			s.StartService();

			var p = new GainCapitalRatesParser(_logger);
			var m = new GainCapitalRatesManager(_logger, s, p);

			var x = m.GetRate("EUR/USD");

			while (true)
			{}
		}

		[Test]
		public void CheckRatesSpreadService()
		{
			var provider = new RatesSpreadService(_logger);
			var spread = provider.GetSpread("EUR/USD", 100);
		}

		[Test]
		public void RunTrueFXRatesService()
		{
			var p = new TrueFXRatesParser(_logger);
			var s = new TrueFXRatesService(_logger, p);
			s.StartProcessing();

			while (true)
			{}
		}
	}
}
