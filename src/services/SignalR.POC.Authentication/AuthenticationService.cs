// =================================================
// File:
// SignalR.POC/SignalR.POC.Authentication/AuthenticationService.cs
// 
// Last updated:
// 2013-08-18 4:10 PM
// =================================================

#region Usings

using SignalR.POC.Authentication.Abstraction;

#endregion

namespace SignalR.POC.Authentication
{
	// Implement authentication logic in this class
	public class AuthenticationService : IAuthenticationService
	{
		public AuthenticationService()
		{}

		public bool AuthenticateUser(string login, string password)
		{
			return true;
		}
	}
}
