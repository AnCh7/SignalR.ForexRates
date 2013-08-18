// =================================================
// File:
// SignalR.POC/SignalR.POC.Logging/IConfiguration.cs
// 
// Last updated:
// 2013-08-18 4:04 PM
// =================================================

#region Usings

using NLog.Config;

#endregion

namespace SignalR.POC.Logging.Abstraction
{
	public interface IConfiguration
	{
		LoggingConfiguration Initialize();
	}
}
