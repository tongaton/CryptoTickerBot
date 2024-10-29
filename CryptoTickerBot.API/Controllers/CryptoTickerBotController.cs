using CryptoTickerBot.Data.Contracts;
using CryptoTickerBot.Data.Domain.CryptoTicker;
using CryptoTickerBot.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CryptoTickerBot.API.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]/[action]")]
    public class CryptoTickerBotController : ControllerBase
    {

        private readonly ILogger<CryptoTickerBotController> _logger;

        private readonly ICryptoTickerHistoryRepository CryptoTickerHistoryRepository;

        public CryptoTickerBotController(ILogger<CryptoTickerBotController> logger, ICryptoTickerHistoryRepository cryptoTickerHistoryRepository)
        {
            _logger = logger;
            this.CryptoTickerHistoryRepository = cryptoTickerHistoryRepository;
        }

        [HttpGet]
        public async Task<List<CryptoTicker>> GetTickerHistory(DateTime fromDate, DateTime toDate, string ticker, string klines)
        {
            try
            {
                //List<CryptoTicker> cryptoHistory = await CryptoTickerHistoryRepository.Top10Ticker();
                List<CryptoTicker> cryptoHistory = await CryptoTickerHistoryRepository.GetTickerHistories(fromDate, toDate, ticker, klines);

                return cryptoHistory;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting Ticket History");
                throw ex;
            }

        }

        [HttpPost]
        public async Task InsertOrUpdateTickerHistory(DateTime timestamp, string ticker, string klines, decimal price)
        {
            try
            {

                await CryptoTickerHistoryRepository.InsertOrUpdateTickerHistory(timestamp,ticker, klines, price);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while inserting or updating Ticket History");
                throw;
            }
        }
    }
}
