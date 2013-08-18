// =================================================
// File:
// SignalR.POC/SignalR.POC.ConsoleServer/IRatesSpreadProvider.cs
// 
// Last updated:
// 2013-08-08 12:20 AM
// =================================================

#region Usings

using SignalR.POC.Library.Models;

#endregion

namespace SignalR.POC.ConsoleServer.DataProviders.Abstraction
{
	public interface IRatesSpreadProvider
	{
		Spread GetSpread(string pairName, int customerId);
	}
}
