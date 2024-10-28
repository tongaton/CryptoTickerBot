using CryptoTickerBot.Data.Contracts;
using CryptoTickerBot.Data.Domain.CryptoTicker;
using CryptoTickerBot.Data.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static CryptoTickerBot.Data.Helpers.DbQueryReader;

namespace CryptoTickerBot.Data.Repositories
{
    public class CryptoTickerHistoryRepository : ICryptoTickerHistoryRepository
    {
        private readonly CryptoTickerContext _cryptoTickerContext;

        private readonly ILogger<CryptoTickerHistoryRepository> _logger;
        public CryptoTickerHistoryRepository(CryptoTickerContext cryptoTickerContext, ILogger<CryptoTickerHistoryRepository> logger)
        {
            _cryptoTickerContext = cryptoTickerContext;
            _logger = logger;
        }
        public async Task<List<CryptoTicker>> Top10Ticker()
        {
            string query = DbQueryReader.GetInstance().GetQuery(Enum.GetName(typeof(DbNames), DbNames.Erky), ErkyQueryReference.TICKER, ErkyQueryReference.TOP_10_TICKER);

            List<CryptoTicker> tickerList = await _cryptoTickerContext.CryptoTickerHistory.FromSqlRaw(query).ToListAsync();

            return tickerList;
        }
        public async Task<List<CryptoTicker>> GetTickerHistories(DateTime fromDate, DateTime toDate, string ticker, string klines)
        {
            try
            {
                string query = DbQueryReader.GetInstance().GetQuery(Enum.GetName(typeof(DbNames), DbNames.Erky), ErkyQueryReference.TICKER, ErkyQueryReference.GET_TICKER_HISTORY);

                List<CryptoTicker> tickerList = await _cryptoTickerContext.CryptoTickerHistory.FromSqlRaw(query, 
                    new SqlParameter[] { 
                        new SqlParameter("@date_from", fromDate), 
                        new SqlParameter("@date_to", toDate), 
                        new SqlParameter("@ticker", ticker), 
                        new SqlParameter("@klines", klines) })
                    .ToListAsync();

                return tickerList;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while updating Cliente external key");
                throw e;
            }

        }

        public void UpdateOrInsertTickerHistory(DateTime timestamp, string ticker, string kline, decimal price) 
        {
            try
            {
                string query = DbQueryReader.GetInstance().GetQuery(Enum.GetName(typeof(DbNames), DbNames.Erky), ErkyQueryReference.TICKER, ErkyQueryReference.UPDATE_OR_INSERT_TICKER_HISTORY);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while updating or inserting Ticker History");
                throw e;
            }
        }
    }
}
