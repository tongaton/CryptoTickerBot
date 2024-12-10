using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoTickerBot.Core.Interfaces
{
    internal partial interface IClientApi : IDisposable
    {
        CancellationTokenSource Cts { get; }
        bool IsInitialized { get; }
        bool IsRunning { get; }

        event OnUpdateDelegate Changed;
        event OnUpdateDelegate Next;
        event TerminateDelegate Terminate;
        event StartDelegate Start;

    }
}
