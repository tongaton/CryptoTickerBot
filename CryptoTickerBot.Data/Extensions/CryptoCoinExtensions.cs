﻿using CryptoTickerBot.Data.Domain;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace CryptoTickerBot.Data.Extensions
{
    public static class CryptoCoinExtensions
    {
        [DebuggerStepThrough]
        [Pure]
        public static decimal Buy(this CryptoCoin coin,
                                    decimal amountInUsd) => amountInUsd / coin.BuyPrice;

        [DebuggerStepThrough]
        [Pure]
        public static decimal Sell(this CryptoCoin coin,
                                     decimal quantity) => coin.SellPrice * quantity;
    }
}