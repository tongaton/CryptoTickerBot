using CryptoTickerBot.Core.Interfaces;
using JetBrains.Annotations;
using NLog;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoTickerBot.Core
{
    public partial class ClientApi : IClientApi
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public CancellationTokenSource Cts { get; private set; }

        private readonly HttpClient _httpClient;

        public bool IsRunning { get; private set; }
        public bool IsInitialized { get; private set; }

        public ClientApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetTicketHistory(string endpoint)
        {
            var response = await _httpClient.GetStringAsync(endpoint);
            return response;
        }

        public void Dispose()
        {
            Cts?.Dispose();
            _httpClient?.Dispose();
        }

        [UsedImplicitly]
        public event TerminateDelegate Terminate;

        public event StartDelegate Start;

        [UsedImplicitly]
        public event OnUpdateDelegate Changed;

        [UsedImplicitly]
        public event OnUpdateDelegate Next;

    }
}
