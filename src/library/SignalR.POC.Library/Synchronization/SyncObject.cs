// =================================================
// File:
// SignalR.POC/SignalR.POC.Library/SyncObject.cs
// 
// Last updated:
// 2013-08-18 4:11 PM
// =================================================

#region Usings

using System.Threading;

#endregion

namespace SignalR.POC.Library.Synchronization
{
	public static class Sync
	{
		public static ManualResetEvent ConsolePauseEvent = new ManualResetEvent(false);
	}
}
