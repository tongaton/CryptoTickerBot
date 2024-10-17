﻿using CryptoTickerBot.Data.Domain;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable UnusedMemberInSuper.Global

namespace CryptoTickerBot.Core.Interfaces
{
    public delegate Task OnUpdateDelegate(ICryptoExchange exchange,
                                            CryptoCoin coin);

    public interface ICryptoExchange : IObservable<CryptoCoin>, IDisposable
    {
        CryptoExchangeId Id { get; }
        string Name { get; }
        string Url { get; }
        string TickerUrl { get; }
        Dictionary<string, string> SymbolMappings { get; }
        decimal BuyFees { get; }
        decimal SellFees { get; }
        TimeSpan PollingRate { get; }
        TimeSpan CooldownPeriod { get; }
        bool IsStarted { get; }
        DateTime StartTime { get; }
        TimeSpan UpTime { get; }
        DateTime LastUpdate { get; }
        TimeSpan LastUpdateDuration { get; }
        DateTime LastChange { get; }
        TimeSpan LastChangeDuration { get; }
        int Count { get; }
        ImmutableHashSet<string> BaseSymbols { get; }
        Markets Markets { get; }
        IDictionary<string, CryptoCoin> ExchangeData { get; }
        IDictionary<string, decimal> DepositFees { get; }
        IDictionary<string, decimal> WithdrawalFees { get; }
        ImmutableHashSet<IObserver<CryptoCoin>> Observers { get; }

        [Pure]
        CryptoCoin this[string symbol] { get; set; }

        [Pure]
        CryptoCoin this[string baseSymbol,
                          string symbol]
        { get; }

        event OnUpdateDelegate Changed;
        event OnUpdateDelegate Next;

        Task StartReceivingAsync(CancellationToken? ct = null);
        Task StopReceivingAsync();

        void Unsubscribe(IObserver<CryptoCoin> subscription);

        [Pure]
        CryptoCoin GetWithFees(string symbol);

        [Pure]
        string ToTable(params string[] symbols);
    }
}