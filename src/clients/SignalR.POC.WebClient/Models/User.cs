// =================================================
// File:
// SignalR.POC/SignalR.POC.WebClient/User.cs
// 
// Last updated:
// 2013-08-18 4:18 PM
// =================================================

#region Usings

using System.ComponentModel.DataAnnotations;

#endregion

namespace SignalR.POC.WebClient.Models
{
	public class User
	{
		public string Login { get; set; }

		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
