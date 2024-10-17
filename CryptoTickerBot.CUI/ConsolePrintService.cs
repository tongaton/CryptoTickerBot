﻿using Colorful;
using CryptoTickerBot.Core.Abstractions;
using CryptoTickerBot.Core.Interfaces;
using CryptoTickerBot.Data.Domain;
using System.Drawing;
using System.Threading.Tasks;

namespace CryptoTickerBot.CUI
{
    public class ConsolePrintService : BotServiceBase
    {
        public StyleSheet StyleSheet { get; }
        private readonly object consoleLock = new object();

        public ConsolePrintService()
        {
            StyleSheet = new StyleSheet(Color.White);
            StyleSheet.AddStyle(@"Highest Bid = (0|[1-9][\d,]*)?(\.\d+)?(?<=\d)", Color.GreenYellow);
            StyleSheet.AddStyle(@"Lowest Ask = (0|[1-9][\d,]*)?(\.\d+)?(?<=\d)", Color.OrangeRed);
        }

        public override Task OnChangedAsync(ICryptoExchange exchange,
                                              CryptoCoin coin)
        {
            lock (consoleLock)
            {
                Console.Write($"{exchange.Name,-12}", Color.DodgerBlue);
                Console.WriteLineStyled($" | {coin}", StyleSheet);
            }

            return Task.CompletedTask;
        }
    }
}