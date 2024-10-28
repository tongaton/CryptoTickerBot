﻿using CryptoTickerBot.Core.Abstractions;
using CryptoTickerBot.Data.Domain;
using Flurl.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoTickerBot.Core.Exchanges
{
    public class ZebpayExchange : CryptoExchangeBase<ZebpayExchange.TickerDatum>
    {
        public static readonly ImmutableList<string> Symbols =
            ImmutableList<string>.Empty.AddRange(
                new[]
                {
                    "BTC", "TUSD", "ETH", "BCH", "LTC", "XRP", "EOS", "OMG",
                    "TRX", "GNT", "ZRX", "REP", "KNC", "BAT", "AE", "ZIL",
                    "CMT", "NCASH", "BTG"
                }
            );

        public ZebpayExchange() : base(CryptoExchangeId.Zebpay)
        {
        }

        protected override async Task GetExchangeDataAsync(CancellationToken ct)
        {
            await FetchAllAsync(TimeSpan.FromSeconds(2), ct).ConfigureAwait(false);

            while (!ct.IsCancellationRequested)
                await FetchAllAsync(PollingRate, ct).ConfigureAwait(false);
        }

        private async Task FetchAllAsync(TimeSpan frequency,
                                           CancellationToken ct)
        {
            foreach (var symbol in Symbols)
            {
                var url = $"{TickerUrl}{symbol}/inr/";
                var data = await url.GetJsonAsync<TickerDatum>(ct).ConfigureAwait(false);
                Update(data, $"{symbol}INR");
                await Task.Delay(frequency, ct).ConfigureAwait(false);
            }
        }

        protected override void DeserializeData(TickerDatum data,
                                                  string id)
        {
            ExchangeData[id].LowestAsk = data.Buy;
            ExchangeData[id].HighestBid = data.Sell;
            ExchangeData[id].Rate = data.Last;
        }

        public class TickerDatum
        {
            [JsonProperty("pricechange")]
            public decimal PriceChange { get; set; }

            [JsonProperty("volume")]
            public decimal Volume { get; set; }

            [JsonProperty("24hoursHigh")]
            public decimal High { get; set; }

            [JsonProperty("24hoursLow")]
            public decimal Low { get; set; }

            [JsonProperty("market")]
            public decimal Last { get; set; }

            [JsonProperty("buy")]
            public decimal Buy { get; set; }

            [JsonProperty("sell")]
            public decimal Sell { get; set; }

            [JsonProperty("pair")]
            public string Pair { get; set; }

            [JsonProperty("virtualCurrency")]
            public string VirtualCurrency { get; set; }

            [JsonProperty("currency")]
            public string Currency { get; set; }
        }
    }
}