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
            IFormatProvider formatProvider = null)
        {
            if (loggerConfiguration is null)
            {
                throw new ArgumentNullException(nameof(loggerConfiguration));
            }

            return loggerConfiguration.Sink(new DiscordSink(formatProvider, webhookId, webhookToken,
                restrictedToMinimumLevel, botName, avatarURL));
        }
    }
}