using Discord;
using Discord.Webhook;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Serilog.Sinks.Discord
{
    public class DiscordSink : IBatchedLogEventSink
    {
        private readonly IFormatProvider _formatProvider;
        private readonly LogEventLevel _restrictedToMinimumLevel;
        private readonly DiscordWebhookClient _webHook;
        private readonly DiscordSinkOptions _options;

        public DiscordSink(DiscordSinkOptions options)
        {
            _options = options;
            _formatProvider = _options.FormatProvider;
            _restrictedToMinimumLevel = _options.MinimumLogEventLevel;
            _webHook = new DiscordWebhookClient(_options.WebHookId, _options.WebHookToken);
        }
        private async Task SendMessageAsync(LogEvent logEvent)
        {
            if (!ShouldLogMessage(_restrictedToMinimumLevel, logEvent.Level))
                return;

            var matchMessage = logEvent.Exception?.ToString() ?? logEvent.RenderMessage(_formatProvider);

            if (ShouldFilterMessage(logEvent, _formatProvider, _options))
            {
                return;
            }

            var embedBuilder = new EmbedBuilder();

            try
            {
                if (logEvent.Exception != null)
                {
                    embedBuilder.Color = new Color(255, 0, 0);
                    embedBuilder.WithTitle(":o: Exception");
                    embedBuilder.AddField("Type:", $"```{logEvent.Exception.GetType().FullName}```");

                    var message = FormatMessage(logEvent.Exception.Message, 1000);
                    embedBuilder.AddField("Message:", message);

                    var stackTrace = FormatMessage(logEvent.Exception.StackTrace, 1000);
                    embedBuilder.AddField("StackTrace:", stackTrace);
                }
                else
                {
                    var message = logEvent.RenderMessage(_formatProvider);
                    message = FormatMessage(message, 240);
                    SpecifyEmbedLevel(logEvent.Level, embedBuilder);
                    embedBuilder.Description = message;
                }

                await _webHook.SendMessageAsync(null, false, new Embed[] { embedBuilder.Build() }, _options.BotName,
                    _options.AvatarURL);
            }

            catch (Exception ex)
            {
                await _webHook.SendMessageAsync($"ooo snap, {ex.Message}", false);
            }
        }
        private static void SpecifyEmbedLevel(LogEventLevel level, EmbedBuilder embedBuilder)
        {
            switch (level)
            {
                case LogEventLevel.Verbose:
                    embedBuilder.Title = ":loud_sound: Verbose";
                    embedBuilder.Color = Color.LightGrey;
                    break;
                case LogEventLevel.Debug:
                    embedBuilder.Title = ":mag: Debug";
                    embedBuilder.Color = Color.LightGrey;
                    break;
                case LogEventLevel.Information:
                    embedBuilder.Title = ":information_source: Information";
                    embedBuilder.Color = new Color(0, 186, 255);
                    break;
                case LogEventLevel.Warning:
                    embedBuilder.Title = ":warning: Warning";
                    embedBuilder.Color = new Color(255, 204, 0);
                    break;
                case LogEventLevel.Error:
                    embedBuilder.Title = ":x: Error";
                    embedBuilder.Color = new Color(255, 0, 0);
                    break;
                case LogEventLevel.Fatal:
                    embedBuilder.Title = ":skull_crossbones: Fatal";
                    embedBuilder.Color = Color.DarkRed;
                    break;
                default:
                    break;
            }
        }

        public static string FormatMessage(string message, int maxLenght)
        {
            if (message.Length > maxLenght)
                message = $"{message.Substring(0, maxLenght)} ...";

            if (!string.IsNullOrWhiteSpace(message))
                message = $"```{message}```";

            return message;
        }

        private static bool ShouldFilterMessage(LogEvent logEvent, IFormatProvider formatProvider, DiscordSinkOptions options)
        {
            var exception = logEvent.Exception?.ToString();
            var message = logEvent.RenderMessage(formatProvider);

            if (exception is not null)
            {
                if (options.FilterExceptions)
                {
                    return ShouldFilterMessage(exception, options) && ShouldFilterMessage(message, options);
                }

                return false;
            }
            return ShouldFilterMessage(message, options);
        }
        
        private static bool ShouldFilterMessage(string message, DiscordSinkOptions options)
        {
            if (options.PrefixFilter is null && options.SuffixFilter is null && options.RegexFilter is null)
            {
                return false;
            }
            
            if (options.PrefixFilter is { } prefix && message.StartsWith(prefix))
            {
                return false;
            }
            if (options.SuffixFilter is { } suffix && message.EndsWith(suffix))
            {
                return false;
            }
            if (options.RegexFilter is { } regex && Regex.IsMatch(message, regex, RegexOptions.IgnoreCase) )
            {
                return false;
            }

            return true;
        }

        private static bool ShouldLogMessage(
            LogEventLevel minimumLogEventLevel,
            LogEventLevel messageLogEventLevel) =>
                (int)messageLogEventLevel >= (int)minimumLogEventLevel;

        public async Task EmitBatchAsync(IReadOnlyCollection<LogEvent> batch)
        {
            foreach (var logEvent in batch)
            {
                await SendMessageAsync(logEvent);
            }
        }

        public Task OnEmptyBatchAsync()
        {
            return Task.CompletedTask;
        }
    }
}
