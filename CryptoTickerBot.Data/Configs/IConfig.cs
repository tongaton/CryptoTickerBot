﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CryptoTickerBot.Data.Configs
{
    public interface IConfig<out TConfig> where TConfig : IConfig<TConfig>
    {
        [JsonIgnore]
        string ConfigFileName { get; }

        [JsonIgnore]
        string ConfigFolderName { get; }

        bool TryValidate(out IList<Exception> exceptions);

        TConfig RestoreDefaults();
    }
}