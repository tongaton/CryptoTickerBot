﻿using CryptoTickerBot.Data.Extensions;
using MoreLinq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CryptoTickerBot.Telegram.Extensions
{
    public static class TelegramBotClientExtensions
    {
        private static async Task<Message> SendTextMessageNoThrowAsync(this TelegramBotClient client,
                                                                         ChatId chatId,
                                                                         string text,
                                                                         ParseMode parseMode = ParseMode.Default,
                                                                         bool disableWebPagePreview = false,
                                                                         bool disableNotification = false,
                                                                         int replyToMessageId = 0,
                                                                         IReplyMarkup replyMarkup = null,
                                                                         CancellationToken cancellationToken = default)
        {
            try
            {
                return await client.SendTextMessageAsync(  chatId, text,parseMode, disableWebPagePreview,
                                                           disableNotification, replyToMessageId, replyMarkup,
                                                           cancellationToken).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                return null;
            }
        }

        public static async Task<Message> SendTextBlockAsync(this TelegramBotClient client,
                                                               ChatId chatId,
                                                               string text,
                                                               bool disableWebPagePreview = false,
                                                               bool disableNotification = false,
                                                               int replyToMessageId = 0,
                                                               IReplyMarkup replyMarkup = null,
                                                               CancellationToken cancellationToken = default)
        {
            try
            {
                return await client.SendTextMessageNoThrowAsync(chatId,
                                                                  text.ToMarkdown(), ParseMode.Markdown,
                                                                  disableWebPagePreview, disableNotification,
                                                                  replyToMessageId,
                                                                  replyMarkup,
                                                                  cancellationToken).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                return null;
            }
        }

        public static async Task SendTextBlocksAsync(this TelegramBotClient client,
                                                       ChatId chatId,
                                                       string text,
                                                       bool disableWebPagePreview = false,
                                                       bool disableNotification = false,
                                                       int replyToMessageId = 0,
                                                       IReplyMarkup replyMarkup = null,
                                                       CancellationToken cancellationToken = default)
        {
            foreach (var chunk in text.SplitOnLength(4000))
                await client.SendTextMessageNoThrowAsync(chatId,
                                                           chunk.ToMarkdown(), ParseMode.Markdown,
                                                           disableWebPagePreview, disableNotification,
                                                           replyToMessageId,
                                                           replyMarkup,
                                                           cancellationToken).ConfigureAwait(false);
        }

        public static async Task<Message> SendOptionsAsync<T>(this TelegramBotClient client,
                                                                ChatId chatId,
                                                                string text,
                                                                IEnumerable<T> options,
                                                                CancellationToken cancellationToken = default)
        {
            var keyboard = new ReplyKeyboardMarkup(
                options.Select(x => new KeyboardButton(x.ToString())),
                true, true
            );

            return await client.SendTextMessageNoThrowAsync(chatId,
                                                              text,
                                                              disableNotification: true,
                                                              replyMarkup: keyboard,
                                                              cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public static async Task<Message> SendOptionsAsync<T>(this TelegramBotClient client,
                                                                ChatId chatId,
                                                                string text,
                                                                IEnumerable<T> options,
                                                                int batchSize = 2,
                                                                CancellationToken cancellationToken = default) =>
            await client.SendOptionsAsync(chatId, text, options.Batch(batchSize), cancellationToken)
                .ConfigureAwait(false);

        public static async Task<Message> SendOptionsAsync<T>(this TelegramBotClient client,
                                                                ChatId chatId,
                                                                string text,
                                                                IEnumerable<IEnumerable<T>> options,
                                                                CancellationToken cancellationToken = default)
        {
            var keyboard = new ReplyKeyboardMarkup(
                options.Select(x => x.Select(y => new KeyboardButton(y.ToString()))),
                true, true);
            return await client.SendTextMessageNoThrowAsync(chatId,
                                                              text,
                                                              disableNotification: true,
                                                              replyMarkup: keyboard,
                                                              cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public static async Task<Message> SendOptionsAsync<T>(this TelegramBotClient client,
                                                                ChatId chatId,
                                                                User user,
                                                                string text,
                                                                IEnumerable<IEnumerable<T>> options,
                                                                CancellationToken cancellationToken = default)
        {
            var keyboard = new ReplyKeyboardMarkup(
                    options.Select(x => x.Select(y => new KeyboardButton(y.ToString()))),
                    true, true)
            { Selective = true };
            return await client.SendTextMessageNoThrowAsync(chatId,
                                                              $"{user}\n{text}",
                                                              disableNotification: true,
                                                              replyMarkup: keyboard,
                                                              cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
    }
}