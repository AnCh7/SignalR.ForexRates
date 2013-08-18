// =================================================
// File:
// SignalR.POC/SignalR.POC.Logging/LoggerWrapper.cs
// 
// Last updated:
// 2013-08-18 4:04 PM
// =================================================

#region Usings

using System;

using NLog;
using NLog.Config;

using SignalR.POC.Logging.Abstraction;

#endregion

namespace SignalR.POC.Logging
{
	public class LoggerWrapper : ILoggerWrapper
	{
		public LoggerWrapper(LoggingConfiguration configuration, string currentClassName)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException("configuration");
			}

			ConfigureLogger(configuration);
			InitializeLogger(currentClassName);
		}

		public Logger Log { get; private set; }

		private void ConfigureLogger(LoggingConfiguration config)
		{
			LogManager.Configuration = config;
		}

		private void InitializeLogger(string currentClassName)
		{
			Log = LogManager.GetLogger(currentClassName);
		}
	}
}
