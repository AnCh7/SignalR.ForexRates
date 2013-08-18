// =================================================
// File:
// SignalR.POC/SignalR.POC.Library/CurrencyPair.cs
// 
// Last updated:
// 2013-08-18 4:12 PM
// =================================================

#region Usings

// =================================================
// File:
// SignalR.POC/SignalR.POC.Library/CurrencyPair.cs
// 
// Last updated:
// 2013-08-05 5:53 PM
// =================================================

#region Usings

#endregion

#endregion

namespace SignalR.POC.Library.Models
{
	public class CurrencyPair
	{
		public CurrencyPair()
		{}

		public CurrencyPair(string pairName, decimal bid, decimal ask, decimal spreadBid, decimal spreadAsk)
		{
			PairName = pairName;
			Bid = bid;
			Ask = ask;
			SpreadBid = spreadBid;
			SpreadAsk = spreadAsk;
		}

		public string PairName { get; set; }

		public string PairNameShort
		{
			get
			{
				return PairName.Replace("/", string.Empty);
			}
		}

		public decimal Bid { get; set; }
		public decimal Ask { get; set; }

		public decimal SpreadBid { get; set; }
		public decimal SpreadAsk { get; set; }
	}
}
