using System;
using Serilog.Configuration;
using Serilog.Events;

namespace Serilog.Sinks.Discord
{
    public static class DiscordSinkExtensions
    {
        public static LoggerConfiguration Discord(
            this LoggerSinkConfiguration loggerConfiguration,
            UInt64 webhookId,
            string webhookToken,
            LogEventLevel restrictedToMinimumLevel = LogEventLevel.Debug,
            string botName = null,
            string avatarURL = null,
            int batchIntervalInSeconds = 2,
            int batchSizeLimit = 100,
            int? queueLimit = null,
            IFormatProvider formatProvider = null)
        {
            if (loggerConfiguration is null)
            {
                throw new ArgumentNullException(nameof(loggerConfiguration));
            }
            
            var batchingOptions = new BatchingOptions
            {
                BatchSizeLimit = batchSizeLimit,
                BufferingTimeLimit = TimeSpan.FromSeconds(batchIntervalInSeconds),
                EagerlyEmitFirstEvent = true,
                QueueLimit = queueLimit
            };

            var options = new DiscordSinkOptions
            {
                BotName = botName,
                AvatarURL = avatarURL,
                WebHookId = webhookId,
                WebHookToken = webhookToken,
                FormatProvider = formatProvider,
                MinimumLogEventLevel = restrictedToMinimumLevel
            };

            return loggerConfiguration.Sink(new DiscordSink(options), batchingOptions);
        }
    }
}