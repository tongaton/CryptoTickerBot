﻿using CryptoTickerBot.Data.Domain;
using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoTickerBot.Core.Interfaces
{
    public interface IBot : IDisposable
    {
        ICryptoExchange this[CryptoExchangeId index] { get; }

        CancellationTokenSource Cts { get; }
        ImmutableDictionary<CryptoExchangeId, ICryptoExchange> Exchanges { get; }
        bool IsInitialized { get; }
        bool IsRunning { get; }
        ImmutableHashSet<IBotService> Services { get; }
        DateTime StartTime { get; }

        event OnUpdateDelegate Changed;
        event OnUpdateDelegate Next;
        event TerminateDelegate Terminate;
        event StartDelegate Start;

        Task StartAsync(CancellationTokenSource cts = null);

        Task StartAsync(CancellationTokenSource cts = null,
                          params CryptoExchangeId[] exchangeIds);

        Task StopAsync();
        void RestartExchangeMonitors();

        bool ContainsService(IBotService service);
        Task AttachAsync(IBotService service);
        Task DetachAsync(IBotService service);
        Task DetachAllAsync<T>() where T : IBotService;

        bool TryGetExchange(CryptoExchangeId exchangeId,
                              out ICryptoExchange exchange);

        bool TryGetExchange(string exchangeId,
                              out ICryptoExchange exchange);
    }

    public delegate void TerminateDelegate(Bot bot);

    public delegate Task StartDelegate(Bot bot);
}