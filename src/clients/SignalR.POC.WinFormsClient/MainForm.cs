// =================================================
// File:
// SignalR.POC/SignalR.POC.WinFormsClient/MainForm.cs
// 
// Last updated:
// 2013-08-18 4:13 PM
// =================================================

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.AspNet.SignalR.Client.Hubs;
using Microsoft.Practices.Unity;

using SignalR.POC.Authentication.Abstraction;
using SignalR.POC.CustomerInfo.Abstraction;
using SignalR.POC.Library.Models;
using SignalR.POC.WinFormsClient.Model;
using SignalR.POC.WinFormsClient.Resolver;

#endregion

namespace SignalR.POC.WinFormsClient
{
	public partial class MainForm : Form
	{
		private readonly object _lockObject = new object();
		private readonly BindingSource _pairsBindingSource;

		private readonly string _url;
		private int _customerId;

		public MainForm()
		{
			InitializeComponent();

			_url = ConfigurationManager.AppSettings["ServerAddress"];

			_pairsBindingSource = new BindingSource();

			btnStart.Enabled = false;
		}

		private async Task RunAsync()
		{
			try
			{
				var hubConnection = new HubConnection(_url);
				hubConnection.Closed += () => SetText(@"[Closed]" + Environment.NewLine);
				hubConnection.ConnectionSlow += () => SetText(@"[ConnectionSlow]" + Environment.NewLine);
				hubConnection.Error += error => SetText(error + Environment.NewLine);
				hubConnection.Reconnected += () => SetText(DateTime.Now + @"[Reconnected]" + Environment.NewLine);
				hubConnection.Reconnecting += () => SetText(DateTime.Now + @"[Reconnecting]" + Environment.NewLine);
				hubConnection.TransportConnectTimeout = new TimeSpan(0, 0, 60);
				hubConnection.StateChanged +=
					change => SetText(DateTime.Now + @"[StateChanged] " + change.OldState + change.NewState + Environment.NewLine);

				// Create hub proxy
				var hubProxy = hubConnection.CreateHubProxy("FXCMRatesHub");

				hubProxy["customerId"] = _customerId;

				// Register methods
				hubProxy.On<IEnumerable<CurrencyPair>>("GetFirstQuotes",
													   pairs =>
													   {
														   foreach (var currencyPair in pairs)
														   {
															   SetText(currencyPair + Environment.NewLine);
														   }
													   });

				hubProxy.On<CurrencyPair>("UpdateMarketPrice",
										  param =>
										  {
											  try
											  {
												  EventHandler h = (sender, args) =>
																   {
																	   lock (_lockObject)
																	   {
																		   var customerList = _pairsBindingSource.DataSource as BindingList<PairData>;
																		   if (customerList != null)
																		   {
																			   var item = customerList.FirstOrDefault(e => e.PairName == param.PairName);
																			   if (item != null)
																			   {
																				   item.Ask = param.Ask;
																				   item.Bid = param.Bid;
																				   item.SpreadAsk = param.SpreadAsk;
																				   item.SpreadBid = param.SpreadBid;
																			   }
																		   }
																	   }
																   };
												  if (InvokeRequired)
												  {
													  BeginInvoke(h);
												  }
												  else
												  {
													  h(null, EventArgs.Empty);
												  }
											  }
											  catch (Exception ex)
											  {
												  SetText(ex + Environment.NewLine);
											  }
										  });

				hubProxy.On("StartQuoting", () => hubProxy.Invoke("StartQuoting"));

				await hubConnection.Start();

				// Invoke methods
				var quotes = await hubProxy.Invoke<IEnumerable<CurrencyPair>>("GetFirstQuotes");
				var dataList = new BindingList<PairData>();
				foreach (var q in quotes.OrderBy(e => e.PairName).ToList())
				{
					dataList.Add(new PairData(q.PairName,
											  q.Ask.ToString(CultureInfo.InvariantCulture),
											  q.Bid.ToString(CultureInfo.InvariantCulture)));
				}

				_pairsBindingSource.DataSource = dataList;
				dataGridView.DataSource = _pairsBindingSource;

				await hubProxy.Invoke("StartQuoting");
			}
			catch (Exception ex)
			{
				SetText(ex + Environment.NewLine);
			}
		}

		private void SetText(string text)
		{
			if (txtLog.InvokeRequired)
			{
				SetTextCallback d = SetText;
				Invoke(d, new object[] {text});
			}
			else
			{
				txtLog.Text += text;
				txtLog.SelectionStart = txtLog.Text.Length;
				txtLog.ScrollToCaret();
			}
		}

		private async void btnStart_Click(object sender, EventArgs e)
		{
			await RunAsync();
		}

		private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			try
			{
				lock (_lockObject)
				{
					var customerList = _pairsBindingSource.DataSource as BindingList<PairData>;
					if (customerList != null)
					{
						var item = customerList.FirstOrDefault(v => v.PairName == (string)dataGridView.Rows[e.RowIndex].Cells[0].Value);
						if (item != null)
						{
							foreach (DataGridViewCell cell in dataGridView.Rows[e.RowIndex].Cells)
							{
								cell.Style.BackColor = item.Color;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				SetText(ex + Environment.NewLine);
			}
		}

		private void btnLogin_Click(object sender, EventArgs e)
		{
			try
			{
				var customerInfo = DependencyFactory.Container.Resolve<ICustomerService>();
				var authenticationService = DependencyFactory.Container.Resolve<IAuthenticationService>();

				var result = authenticationService.AuthenticateUser(txtLogin.Text, txtPassword.Text);

				if (result)
				{
					_customerId = customerInfo.GetCustomerId(txtLogin.Text, txtPassword.Text);

					btnStart.Enabled = true;
					btnLogin.Enabled = false;

					SetText(@"Authentication was successful" + Environment.NewLine);
					SetText(@"CustomerCode = " + _customerId + Environment.NewLine);
				}
				else
				{
					SetText(@"Invalid credentials." + Environment.NewLine);
				}
			}
			catch (Exception ex)
			{
				SetText(ex + Environment.NewLine);
			}
		}

		private delegate void SetTextCallback(string text);
	}
}
