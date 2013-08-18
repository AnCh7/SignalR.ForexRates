// =================================================
// File:
// SignalR.POC/SignalR.POC.ConsoleServer/IRatesProvider.cs
// 
// Last updated:
// 2013-08-08 12:23 AM
// =================================================

#region Usings

#endregion

#region Usings

using System.Collections.Generic;

using SignalR.POC.Library.Models;

#endregion

namespace SignalR.POC.ConsoleServer.DataProviders.Abstraction
{
	public interface IRatesProvider
	{
		IEnumerable<CurrencyPair> GetFirstQuotes(int customerId);

		void StartQuoting(string connectionId, int customerId);

		void StopQuoting(string connectionId);
	}
}
