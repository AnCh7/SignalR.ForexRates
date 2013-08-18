// =================================================
// File:
// SignalR.POC/SignalR.POC.RatesGainCapital/StateObject.cs
// 
// Last updated:
// 2013-08-18 4:07 PM
// =================================================

#region Usings

using System.Net.Sockets;
using System.Text;

#endregion

namespace SignalR.POC.RatesGainCapital.Models
{
	public class StateObject
	{
		public const int BufferSize = 2048;

		public byte[] Buffer = new byte[BufferSize];

		public StringBuilder StrBuilder = new StringBuilder();

		public Socket WorkSocket = null;
	}
}
