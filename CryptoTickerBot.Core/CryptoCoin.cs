﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using CryptoTickerBot.Core.Helpers;
using Newtonsoft.Json;

namespace CryptoTickerBot.Core
{
	public class CryptoCoin : IEquatable<CryptoCoin>
	{
		public string Symbol { get; }
		public decimal HighestBid { get; set; }
		public decimal LowestAsk { get; set; }
		public decimal Rate { get; set; }
		public DateTime Time { get; set; }

		[JsonIgnore]
		public decimal SellPrice => HighestBid;

		[JsonIgnore]
		public decimal BuyPrice => LowestAsk;

		[JsonIgnore]
		public decimal Average => ( BuyPrice + SellPrice ) / 2;

		[JsonIgnore]
		public decimal Spread => BuyPrice - SellPrice;

		[JsonIgnore]
		public decimal SpreadPercentage => Average != 0 ? Spread / Average : 0;

		public CryptoCoin (
			string symbol,
			decimal highestBid = 0m,
			decimal lowestAsk = 0m,
			decimal rate = 0m,
			DateTime? time = null
		)
		{
			Symbol     = symbol;
			HighestBid = highestBid;
			LowestAsk  = lowestAsk;
			Rate       = rate;
			Time       = time ?? DateTime.UtcNow;
		}

		public bool Equals ( CryptoCoin other ) =>
			other != null && Symbol == other.Symbol &&
			HighestBid == other.HighestBid && LowestAsk == other.LowestAsk;

		[DebuggerStepThrough]
		[Pure]
		public virtual decimal Buy ( decimal amountInUsd ) => amountInUsd / BuyPrice;

		[DebuggerStepThrough]
		[Pure]
		public virtual decimal Sell ( decimal quantity ) => SellPrice * quantity;

		public override bool Equals ( object obj ) => Equals ( obj as CryptoCoin );

		public override int GetHashCode ( ) =>
			-1758840423 + EqualityComparer<string>.Default.GetHashCode ( Symbol );

		[DebuggerStepThrough]
		[Pure]
		public CryptoCoin Clone ( ) =>
			new CryptoCoin ( Symbol, HighestBid, LowestAsk, Rate, Time );

		[DebuggerStepThrough]
		public static bool operator == ( CryptoCoin coin1,
		                                 CryptoCoin coin2 ) =>
			EqualityComparer<CryptoCoin>.Default.Equals ( coin1, coin2 );

		[DebuggerStepThrough]
		public static bool operator != ( CryptoCoin coin1,
		                                 CryptoCoin coin2 ) =>
			!( coin1 == coin2 );

		public static PriceChange operator - ( CryptoCoin coin1,
		                                       CryptoCoin coin2 ) =>
			PriceChange.Difference ( coin1, coin2 );

		[Pure]
		public override string ToString ( ) =>
			$"{Symbol,-9}: Highest Bid = {HighestBid,-10:N} Lowest Ask = {LowestAsk,-10:N}";
	}
}