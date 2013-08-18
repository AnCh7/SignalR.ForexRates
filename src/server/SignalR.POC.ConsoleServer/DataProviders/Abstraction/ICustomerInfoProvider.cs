// =================================================
// File:
// SignalR.POC/SignalR.POC.ConsoleServer/ICustomerInfoProvider.cs
// 
// Last updated:
// 2013-08-08 12:22 AM
// =================================================

#region Usings

#endregion

#region Usings

using System.Collections.Generic;

#endregion

namespace SignalR.POC.ConsoleServer.DataProviders.Abstraction
{
	public interface ICustomerInfoProvider
	{
		List<string> GetListOfPairNames(int customerId);
	}
}
