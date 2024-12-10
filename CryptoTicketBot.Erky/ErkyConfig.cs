using Castle.Core.Internal;
using CoinbasePro.WebSocket.Models.Response;
using CryptoTickerBot.Data.Configs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoTickerBot.Erky
{
    public class ErkyConfig : IConfig<ErkyConfig>
    {
        public string ConfigFileName { get; } = "Erky";
        public string ConfigFolderName { get; } = "Configs";
        public string InternalAPI { get; set; }
        public string InsertOrUpdateEndpoint { get; set; }
        public int RetryLimit { get; set; } = 5;
        public TimeSpan RetryInterval { get; set; } = TimeSpan.FromSeconds(5);
        public TimeSpan UpdateFrequency { get; set; } = TimeSpan.FromSeconds(6);
        public Dictionary<string, string> ErkyMappings { get; set; }

        public ErkyConfig RestoreDefaults() =>
            new ErkyConfig
            {
                InternalAPI = InternalAPI,
                InsertOrUpdateEndpoint = InsertOrUpdateEndpoint,
                ErkyMappings = ErkyMappings
            };
        public bool TryValidate(out IList<Exception> exceptions)
        {
            exceptions = new List<Exception>();

            if (string.IsNullOrEmpty(InternalAPI))
                exceptions.Add(new ArgumentException("Erky API missing", nameof(InternalAPI)));

            if (string.IsNullOrEmpty(InsertOrUpdateEndpoint))
                exceptions.Add(new ArgumentException("Erky Insert or Update endpoint missing", nameof(InsertOrUpdateEndpoint)));

            ErkyMappings = new Dictionary<string, string>(ErkyMappings);

            return !exceptions.Any();
        }
    }
}
