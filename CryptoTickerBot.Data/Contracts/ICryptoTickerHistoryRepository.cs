using System;
using CryptoTickerBot.Data.Domain.CryptoTicker;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTickerBot.Data.Contracts
{
    public interface ICryptoTickerHistoryRepository
    {
        public Task<List<CryptoTicker>> Top10Ticker();
        public Task<List<CryptoTicker>> GetTickerHistories(DateTime fromDate, DateTime toDate, string ticker, string klines);
        public Task InsertOrUpdateTickerHistory(DateTime timestamp, string ticker, string klines, decimal price);
    }
}
