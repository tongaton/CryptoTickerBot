using CryptoTickerBot.Data.Contracts;
using CryptoTickerBot.Data.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using static CryptoTickerBot.Data.Helpers.DbQueryReader;

namespace CryptoTickerBot.Data.Repositories
{
    public class CryptoTickerHistoryRepository : ICryptoTickerHistoryRepository
    {
        private readonly CryptoTickerContext _cryptoTickerContext
            ;
        private readonly ILogger<CryptoTickerHistoryRepository> _logger;
        public CryptoTickerHistoryRepository(CryptoTickerContext cryptoTickerContext, ILogger<CryptoTickerContext> logger)
        {
            _cryptoTickerContext = cryptoTickerContext;
            _logger = (ILogger<CryptoTickerHistoryRepository>)logger;
        }
        public void GetTickerHistory(int ClientID, int subCount, string externalKey)
        {
            try
            {
                string query = DbQueryReader.GetInstance().GetQuery(Enum.GetName(typeof(DbNames), DbNames.Erky), ErkyQueryReference.TICKER, ErkyQueryReference.GET_TICKER_HISTORY);
                
                var result = _cryptoTickerContext.Database.ExecuteSqlRaw(query, new SqlParameter[] { new SqlParameter("@client_id", ClientID), new SqlParameter("@sub_count", subCount), new SqlParameter("@external_key", externalKey) });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while updating Cliente external key");
                throw e;
            }

        }
    }
}
