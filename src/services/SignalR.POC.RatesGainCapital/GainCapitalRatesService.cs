// =================================================
// File:
// SignalR.POC/SignalR.POC.RatesGainCapital/GainCapitalRatesService.cs
// 
// Last updated:
// 2013-08-18 4:07 PM
// =================================================

#region Usings

using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using SignalR.POC.Logging.Abstraction;
using SignalR.POC.RatesGainCapital.Abstraction;
using SignalR.POC.RatesGainCapital.Models;

#endregion

namespace SignalR.POC.RatesGainCapital
{
	public class GainCapitalRatesService : IGainCapitalRatesService
	{
		private readonly ManualResetEvent _connectDone = new ManualResetEvent(false);
		private readonly ManualResetEvent _receiveDone = new ManualResetEvent(false);
		private readonly ManualResetEvent _sendDone = new ManualResetEvent(false);

		private readonly string _url;
		private readonly int _port;

		private readonly ILoggerWrapper _wrapper;

		private Socket _client;

		private ConcurrentQueue<string> _response = new ConcurrentQueue<string>();
		private string _temp;

		public GainCapitalRatesService(ILoggerWrapper wrapper)
		{
			_wrapper = wrapper;

			_port = Int32.Parse(ConfigurationManager.AppSettings["GainCapitalRatesPort"]);
			_url = ConfigurationManager.AppSettings["GainCapitalRatesUrl"];

			StartService();
		}

		public void StartService()
		{
			try
			{
				var ipHostInfo = Dns.GetHostEntry(_url);
				var ipAddress = ipHostInfo.AddressList[0];
				var remoteEndPoint = new IPEndPoint(ipAddress, _port);

				_client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				_client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

				_client.BeginConnect(remoteEndPoint, ConnectCallback, _client);
				_connectDone.WaitOne();

				Send(_client, ConfigurationManager.AppSettings["GainCapitalRatesToken"]);
				_sendDone.WaitOne();

				Receive(_client);
				_receiveDone.WaitOne();
			}
			catch (Exception ex)
			{
				_wrapper.Log.Error(ex.Message);
				Console.WriteLine(ex.Message);
			}
		}

		public void StopService()
		{
			try
			{
				_client.Shutdown(SocketShutdown.Both);
				_client.Close();
				_response = null;
			}
			catch (Exception ex)
			{
				_wrapper.Log.Error(ex.Message);
				Console.WriteLine(ex.Message);
			}
		}

		public string GetResponse()
		{
			string data;
			_response.TryDequeue(out data);
			return data;
		}

		private void ConnectCallback(IAsyncResult ar)
		{
			try
			{
				var client = (Socket)ar.AsyncState;
				client.EndConnect(ar);
				Console.WriteLine("Socket connected to {0}", client.RemoteEndPoint);
				_connectDone.Set();
			}
			catch (Exception ex)
			{
				_wrapper.Log.Error(ex.Message);
				Console.WriteLine(ex.Message);
			}
		}

		private void Receive(Socket client)
		{
			try
			{
				var state = new StateObject();
				state.WorkSocket = client;
				client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, state);

				_temp = state.StrBuilder.ToString();
			}
			catch (Exception ex)
			{
				_wrapper.Log.Error(ex.Message);
				Console.WriteLine(ex.Message);
			}
		}

		private void ReceiveCallback(IAsyncResult ar)
		{
			try
			{
				var state = (StateObject)ar.AsyncState;
				var client = state.WorkSocket;
				var bytesRead = client.EndReceive(ar);

				if (bytesRead > 0)
				{
					state.StrBuilder.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));
					client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, state);

					_temp += state.StrBuilder.ToString();
					state.StrBuilder.Clear();

					_receiveDone.Set();
				}
				else
				{
					if (state.StrBuilder.Length > 1)
					{
						_temp = state.StrBuilder.ToString();
					}

					_receiveDone.Set();
				}
			}
			catch (Exception ex)
			{
				_wrapper.Log.Error(ex.Message);
				Console.WriteLine(ex.Message);
			}

			_response.Enqueue(_temp);
		}

		private void Send(Socket client, String data)
		{
			var byteData = Encoding.ASCII.GetBytes(data);
			client.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, client);
		}

		private void SendCallback(IAsyncResult ar)
		{
			try
			{
				var client = (Socket)ar.AsyncState;
				var bytesSent = client.EndSend(ar);
				Console.WriteLine("Sent {0} bytes to server.", bytesSent);
				_sendDone.Set();
			}
			catch (Exception ex)
			{
				_wrapper.Log.Error(ex.Message);
				Console.WriteLine(ex.Message);
			}
		}
	}
}
