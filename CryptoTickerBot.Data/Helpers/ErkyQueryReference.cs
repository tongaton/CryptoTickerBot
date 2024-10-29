using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoTickerBot.Data.Helpers
{
    public class ErkyQueryReference
    {
        //entity name
        public const string TICKER = "TICKER";

        //Query names
        public const string GET_TICKER_HISTORY_LIST = "GET_TICKER_HISTORY_LIST";

        //TOP 10 TICKER 
        public const string TOP_10_TICKER = "TOP_10_TICKER";

        //Insert or update history
        public const string INSERT_OR_UPDATE_TICKER_HISTORY = "INSERT_OR_UPDATE_TICKER_HISTORY";
    }
}
