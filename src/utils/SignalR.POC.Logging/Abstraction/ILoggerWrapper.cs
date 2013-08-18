// =================================================
// File:
// SignalR.POC/SignalR.POC.Logging/ILoggerWrapper.cs
// 
// Last updated:
// 2013-08-18 4:04 PM
// =================================================

#region Usings

using NLog;

#endregion

namespace SignalR.POC.Logging.Abstraction
{
	public interface ILoggerWrapper
	{
		Logger Log { get; }
	}
}
