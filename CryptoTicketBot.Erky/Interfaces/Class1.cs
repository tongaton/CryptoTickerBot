using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoTickerBot.Erky.Interfaces
{
    public interface IErkyBot
    {
        HttpClient Client { get; }
        //User Self { get; }
        ErkyConfig Config { get; }
        Policy Policy { get; }
        DateTime StartTime { get; }
        CancellationToken CancellationToken { get; }
        Task StartAsync();
        void Stop();
    }
}
