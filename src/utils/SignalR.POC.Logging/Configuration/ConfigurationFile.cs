// =================================================
// File:
// SignalR.POC/SignalR.POC.Logging/ConfigurationFile.cs
// 
// Last updated:
// 2013-08-18 4:04 PM
// =================================================

#region Usings

using NLog;
using NLog.Config;
using NLog.Targets;

using SignalR.POC.Logging.Abstraction;

#endregion

namespace SignalR.POC.Logging.Configuration
{
	public class ConfigurationFile : IConfiguration
	{
		public LoggingConfiguration Initialize()
		{
			// Step 1. Create configuration object 
			var config = new LoggingConfiguration();

			// Step 2. Create targets and add them to the configuration 
			var fileTarget = new FileTarget();
			config.AddTarget("file", fileTarget);

			// Step 3. Set target properties 
			//"${basedir}/ForexRates.log";
			fileTarget.FileName = @"D:\ForexRates.log";

			//${longdate} ${logger} ${message}
			fileTarget.Layout = @"${longdate}     ${message}";

			// Step 4. Define rules
			var rule = new LoggingRule("*", LogLevel.Debug, fileTarget);
			config.LoggingRules.Add(rule);

			return config;
		}
	}
}
