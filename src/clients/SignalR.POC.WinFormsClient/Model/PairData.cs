// =================================================
// File:
// SignalR.POC/SignalR.POC.WinFormsClient/PairData.cs
// 
// Last updated:
// 2013-08-18 4:16 PM
// =================================================

#region Usings

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

using SignalR.POC.Library.Models;

#endregion

namespace SignalR.POC.WinFormsClient.Model
{
	public class PairData : CurrencyPair, INotifyPropertyChanged
	{
		private decimal _ask;
		private decimal _bid;
		private DateTime _lastChange = DateTime.MinValue;
		private string _pairName;
		private decimal _spreadAsk;
		private decimal _spreadBid;

		public PairData(string pairName, string ask, string bid)
		{
			_pairName = pairName;
			_ask = decimal.Parse(ask);
			_bid = decimal.Parse(bid);
		}

		public new string PairName
		{
			get
			{
				return _pairName;
			}
			set
			{
				if (_pairName != value)
				{
					_pairName = value;
					OnPropertyChanged();
				}
			}
		}

		public new decimal Bid
		{
			get
			{
				return _bid;
			}
			set
			{
				if (_bid != value)
				{
					PreviousBid = _bid;
					_bid = value;
					_lastChange = DateTime.Now;
					OnPropertyChanged();
				}
			}
		}

		public new decimal Ask
		{
			get
			{
				return _ask;
			}
			set
			{
				if (_ask != value)
				{
					_ask = value;
					OnPropertyChanged();
				}
			}
		}

		public new decimal SpreadAsk
		{
			get
			{
				return _spreadAsk;
			}
			set
			{
				if (_spreadAsk != value)
				{
					_spreadAsk = value;
					OnPropertyChanged();
				}
			}
		}

		public new decimal SpreadBid
		{
			get
			{
				return _spreadBid;
			}
			set
			{
				if (_spreadBid != value)
				{
					_spreadBid = value;
					OnPropertyChanged();
				}
			}
		}

		public decimal PreviousBid { get; private set; }

		public Color Color
		{
			get
			{
				var c = Color.White;

				if ((DateTime.Now - _lastChange).TotalMilliseconds < 500)
				{
					if (PreviousBid > _bid)
					{
						c = Color.Green;
					}
					if (PreviousBid < _bid)
					{
						c = Color.Red;
					}
				}

				return c;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
