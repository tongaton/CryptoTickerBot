using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoTickerBot.Data.Domain.CryptoTicker
{
    public class CryptoTicker
    {
        public int TickerHistoryId { get; set; } 
        public string Ticker { get; set; }
        public DateTime TickerDate { get; set; }
        public decimal Price { get; set; }
        public string KlinesName { get; set; }
        public decimal EMA7 { get; set; }
        public decimal EMA25 { get; set; }
        public decimal EMA99 { get; set; }
    }
}
