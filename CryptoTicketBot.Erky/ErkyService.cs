using static CryptoTickerBot.Core.Exchanges.BinanceExchange;
using CryptoTickerBot.Core.Interfaces;
using CryptoTickerBot.Core.Abstractions;
using CryptoTickerBot.Data.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using SystemHttpClient = System.Net.Http.HttpClient;
using System.Threading.Tasks;
using Flurl.Http;
using NLog;
using JetBrains.Annotations;


namespace CryptoTickerBot.Erky
{
    public class ErkyService : BotServiceBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly SystemHttpClient _erkyClient;
        public ErkyConfig ErkyConfig { get; set; }
        public DateTime LastUpdate { get; private set; } = DateTime.UtcNow;

        public ErkyService (ErkyConfig erkyConfig, SystemHttpClient httpClient)
        {
            ErkyConfig = erkyConfig;

            try
            {
                _erkyClient = httpClient ?? throw new ArgumentNullException(nameof(SystemHttpClient));
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
        [UsedImplicitly]
        public event UpdateDelegate Update;

        public override async Task OnChangedAsync(ICryptoExchange exchange,
                                            CryptoCoin coin)
        {
            //if (DateTime.UtcNow - LastUpdate < ErkyConfig.UpdateFrequency)
            //    return;
            //LastUpdate = DateTime.UtcNow;

            try
            {
                if (ErkyConfig.ErkyMappings.ContainsValue(coin.Symbol))
                {
                    //Console.WriteLine($"{coin.Symbol} exists as a value in SymbolMappings.");

                    var erkyURL = $"{ErkyConfig.InternalAPI}{ErkyConfig.InsertOrUpdateEndpoint}";
                        erkyURL += "?timestamp=" + coin.Time.ToString("yyyy/MM/dd HH:mm:ss");
                        erkyURL += "&ticker=" + coin.Symbol;
                        erkyURL += "&klines=4h";
                        erkyURL += "&price=" + coin.Rate.ToString().Replace(",", ".");

                    var response = await _erkyClient.PostAsync(erkyURL, null);
                    response.EnsureSuccessStatusCode();

                    var responseData = await response.Content.ReadAsStringAsync();

                    Logger.Debug($"Erky Updating @ {responseData}");
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

        }
        private async Task PerformHttpClientTaskAsync(CancellationToken ct)
        {
            try
            {
                var response = await $"{ErkyConfig.InternalAPI}{ErkyConfig.InsertOrUpdateEndpoint}"
                    .GetJsonAsync<List<RestTickerDatum>>(ct)
                    .ConfigureAwait(false);
                //response.EnsureSuccessStatusCode();
                //var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(response);
                // Process the response as needed
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error making HTTP request: {ex.Message}");
                // Log or handle errors
            }
        }

    }

    public delegate Task UpdateDelegate(ErkyService service);

}
