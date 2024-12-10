using CryptoTickerBot.Data.Configs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoTickerBot.API
{
    public class InternalApiConfig : IConfig<InternalApiConfig>
    {
        public string ConfigFileName { get; } = "ErkyBot";
        public string ConfigFolderName { get; } = "Configs";
        public string BotToken { get; set; }
        public int RetryLimit { get; set; } = 5;
        public TimeSpan RetryInterval { get; set; } = TimeSpan.FromSeconds(5);

        public InternalApiConfig RestoreDefaults() =>
            new InternalApiConfig
            {
                BotToken = BotToken
            };
        public bool TryValidate(out IList<Exception> exceptions)
        {
            exceptions = new List<Exception>();

            if (string.IsNullOrEmpty(BotToken))
                exceptions.Add(new ArgumentException("Bot Token missing", nameof(BotToken)));

            return !exceptions.Any();
        }
    }
}
