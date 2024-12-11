using CryptoTickerBot.Core.Abstractions;
using CryptoTickerBot.Data.Domain;
using Flurl.Http;
using Humanizer;
using Humanizer.Localisation;
using Newtonsoft.Json;
using NLog;
using PureWebSockets;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Humanizer;
using Humanizer.Localisation;
using CoinbasePro.Services.Products.Models;

namespace CryptoTickerBot.Core.Exchanges
{
    public class BinanceExchange : CryptoExchangeBase<BinanceExchange.ITickerDatum>
    {
        public const string RestBaseEndpoint = "https://api.binance.com";
        public const string RestTickerEndpoint = "/api/v1/ticker/24hr";
        public const string RestKlinesEndpoint = "/api/v3/klines";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<string, Task> _klineTasks;

        public BinanceExchange() : base(CryptoExchangeId.Binance)
        {
            TickerUrl = $"{TickerUrl}@{PollingRate.TotalMilliseconds}ms";
        }

        protected override async Task FetchInitialDataAsync(CancellationToken ct)
        {
            try
            {
                var data = await $"{RestBaseEndpoint}{RestTickerEndpoint}"
                    .GetJsonAsync<List<RestTickerDatum>>(ct)
                    .ConfigureAwait(false);

                foreach (var datum in data)
                {
                    Update(datum, datum.Symbol);
                }

            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        protected override async Task StartAllKlineUpdates(CancellationToken ct)
        {
            var klineIntervals = new Dictionary<string, TimeSpan>
            {
                { "15m", TimeSpan.FromMinutes(15) },
                { "1h", TimeSpan.FromHours(1) },
                { "4h", TimeSpan.FromHours(4) },
                { "1d", TimeSpan.FromDays(1) }
            };

            var tasks = new List<Task>();

            foreach (var klineInterval in klineIntervals)
            {
                var kline = klineInterval.Key;
                var delay = klineInterval.Value;

                // Start a separate task for this interval
                tasks.Add(Task.Run(async () =>
                      await GetAllKlinesAsync(kline, delay, ct)  
                    , ct));
            }

            try
            {
                // Wait for all kline tasks to complete
                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("All kline updates canceled.");
            }
        }

        protected override async Task GetAllKlinesAsync(string kline, TimeSpan delay, CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    foreach (var ticker in SymbolMappings)
                    {
                        string symbol = ticker.Key;
                        string mappedValue = ticker.Value;

                        Console.WriteLine($"[{DateTime.UtcNow:dddd, yyyy-MM-dd HH:mm:ss}] Updating {symbol} on kline {kline}.");

                        // Run the kline update task
                        await GetKlinesAsync(symbol, kline, ct).ConfigureAwait(false);
                    }

                    // Delay until the next update for this interval
                    if (!ct.IsCancellationRequested)
                    {
                        await Task.Delay(delay, ct).ConfigureAwait(false);
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine($"Cancellation requested for {kline} updates.");
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error in {kline} updates: {ex.Message}");
                }
            }

        }
        protected override async Task GetKlinesAsync(string ticker, string kline, CancellationToken ct)
        {
            try
            {
                // Retrieve last time for the kline
                var startTime = DateTimeOffset.UtcNow.Subtract(TimeSpan.FromHours(12)).ToUnixTimeMilliseconds();
                var klinesUrl = $"{RestBaseEndpoint}{RestKlinesEndpoint}";
                    klinesUrl += $"?interval={kline}";
                    klinesUrl += $"&startTime={startTime}";
                    klinesUrl += "&timeZone=-3";
                    klinesUrl += $"&symbol={ticker}";

                var data = await $"{klinesUrl}"
                    .GetJsonAsync<List<object>>(ct)
                    .ConfigureAwait(false);

                //analyze all klines
                // KlineTickerDatum

            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Delay was canceled.");
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
        protected override async Task GetExchangeDataAsync(CancellationToken ct)
        {
            var options = new PureWebSocketOptions
            {
                DebugMode = false
            };

            using (var ws = new PureWebSocket(TickerUrl, options))
            {
                var closed = false;

                ws.OnMessage += WsOnMessage;
                ws.OnClosed += reason =>
                {
                    Logger.Info($"Binance closed: {reason}");
                    closed = true;
                };
                ws.OnError += exception =>
                {
                    Logger.Error(exception);
                    closed = true;
                };

                if (!ws.Connect())
                    Logger.Error("Couldn't connect to Binance");

                while (ws.State != WebSocketState.Closed)
                {
                    if (UpTime > LastUpdateDuration &&
                         LastUpdateDuration > TimeSpan.FromMinutes(15) ||
                         closed)
                    {
                        ws.Disconnect();
                        break;
                    }

                    await Task.Delay(PollingRate, ct).ConfigureAwait(false);
                }
            }
        }

        protected override void DeserializeData(ITickerDatum datum,
                                                  string id)
        {
            ExchangeData[id].LowestAsk = datum.BestAskPrice;
            ExchangeData[id].HighestBid = datum.BestBidPrice;
            ExchangeData[id].Rate = datum.Close;
        }
        private bool HasKlineMapped(string symbol)
        {
            if (SymbolMappings.ContainsKey(symbol))
                return true;
            else
                return false;
        }

        private void WsOnMessage(string json)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<List<WebsocketTickerDatum>>(json);

                foreach (var datum in data)
                    Update(datum, datum.Symbol);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        public interface ITickerDatum
        {
            string Symbol { get; set; }
            decimal Close { get; set; }
            decimal BestBidPrice { get; set; }
            decimal BestAskPrice { get; set; }
        }

        public class RestTickerDatum : ITickerDatum
        {
            [JsonProperty("symbol")]
            public string Symbol { get; set; }

            [JsonProperty("priceChange")]
            public decimal PriceChange { get; set; }

            [JsonProperty("priceChangePercent")]
            public decimal PriceChangePercent { get; set; }

            [JsonProperty("weightedAvgPrice")]
            public decimal WeightedAvgPrice { get; set; }

            [JsonProperty("prevClosePrice")]
            public decimal PrevClosePrice { get; set; }

            [JsonProperty("lastPrice")]
            public decimal Close { get; set; }

            [JsonProperty("lastQty")]
            public decimal LastQty { get; set; }

            [JsonProperty("bidPrice")]
            public decimal BestBidPrice { get; set; }

            [JsonProperty("askPrice")]
            public decimal BestAskPrice { get; set; }

            [JsonProperty("openPrice")]
            public decimal OpenPrice { get; set; }

            [JsonProperty("highPrice")]
            public decimal HighPrice { get; set; }

            [JsonProperty("lowPrice")]
            public decimal LowPrice { get; set; }

            [JsonProperty("volume")]
            public decimal Volume { get; set; }

            [JsonProperty("quoteVolume")]
            public decimal QuoteVolume { get; set; }

            [JsonProperty("openTime")]
            public long OpenTime { get; set; }

            [JsonProperty("closeTime")]
            public long CloseTime { get; set; }

            [JsonProperty("firstId")]
            public long FirstId { get; set; }

            [JsonProperty("lastId")]
            public long LastId { get; set; }

            [JsonProperty("count")]
            public long Count { get; set; }
        }

        public class WebsocketTickerDatum : ITickerDatum
        {
            [JsonProperty("e")]
            public string EventType { get; set; }

            [JsonProperty("E")]
            public long Time { get; set; }

            [JsonProperty("s")]
            public string Symbol { get; set; }

            [JsonProperty("p")]
            public decimal PriceChange { get; set; }

            [JsonProperty("P")]
            public decimal PriceChangePercent { get; set; }

            [JsonProperty("w")]
            public decimal WeightedAveragePrice { get; set; }

            [JsonProperty("x")]
            public decimal YesterdaysClosePrice { get; set; }

            [JsonProperty("c")]
            public decimal Close { get; set; }

            [JsonProperty("Q")]
            public decimal CloseTradeQuantity { get; set; }

            [JsonProperty("b")]
            public decimal BestBidPrice { get; set; }

            [JsonProperty("B")]
            public decimal BestBidQuantity { get; set; }

            [JsonProperty("a")]
            public decimal BestAskPrice { get; set; }

            [JsonProperty("A")]
            public decimal BestAskQuantity { get; set; }

            [JsonProperty("o")]
            public decimal Open { get; set; }

            [JsonProperty("h")]
            public decimal High { get; set; }

            [JsonProperty("l")]
            public decimal Low { get; set; }

            [JsonProperty("v")]
            public decimal TotalTradedBaseAssetVolume { get; set; }

            [JsonProperty("q")]
            public decimal TotalTradedQuoteAssetVolume { get; set; }

            [JsonProperty("O")]
            public long StatisticsOpenTime { get; set; }

            [JsonProperty("C")]
            public long StatisticsCloseTime { get; set; }

            [JsonProperty("F")]
            public long FirstTradeId { get; set; }

            [JsonProperty("L")]
            public long LastTradeId { get; set; }

            [JsonProperty("n")]
            public long NumberOfTrades { get; set; }
        }

        public class KlineTickerDatum : ITickerDatum
        {
            public string Symbol { get; set; }
            public long OpenTime { get; set; } // Position 0: Open time in milliseconds since Unix epoch
            public decimal OpenPrice { get; set; } // Position 1: Open price
            public decimal HighPrice { get; set; } // Position 2: High price
            public decimal LowPrice { get; set; } // Position 3: Low price
            public decimal Close { get; set; } // Position 4: Close price
            public decimal Volume { get; set; } // Position 5: Volume
            public long CloseTime { get; set; } // Position 6: Close time in milliseconds since Unix epoch
            public decimal QuoteAssetVolume { get; set; } // Position 7: Quote asset volume
            public int NumberOfTrades { get; set; } // Position 8: Number of trades
            public decimal TakerBuyBaseAssetVolume { get; set; } // Position 9: Taker buy base asset volume
            public decimal TakerBuyQuoteAssetVolume { get; set; } // Position 10: Taker buy quote asset volume
            public string Ignore { get; set; } // Position 11: Ignore (always "0")
            public decimal BestBidPrice { get; set; }
            public decimal BestAskPrice { get; set; }
        }
    }
}