using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoTickerBot.Data.Contracts
{
    public interface ICryptoTickerHistoryRepository
    {
        void GetTickerHistory(int ClientID, int subCount, string externalKey);

    }
}
