// =================================================
// File:
// SignalR.POC/SignalR.POC.WinFormsClient/Program.cs
// 
// Last updated:
// 2013-08-18 4:12 PM
// =================================================

#region Usings

using System;
using System.Windows.Forms;

#endregion

namespace SignalR.POC.WinFormsClient
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
