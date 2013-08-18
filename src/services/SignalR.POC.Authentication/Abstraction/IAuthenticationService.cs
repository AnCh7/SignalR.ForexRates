// =================================================
// File:
// SignalR.POC/SignalR.POC.Authentication/IAuthenticationService.cs
// 
// Last updated:
// 2013-08-18 4:10 PM
// =================================================

namespace SignalR.POC.Authentication.Abstraction
{
	public interface IAuthenticationService
	{
		bool AuthenticateUser(string login, string password);
	}
}
