
using System;
using Serilog.Events;

namespace Serilog.Sinks.Discord
{
    public sealed class DiscordSinkOptions
    {
        public UInt64 WebHookId  { get; set; }
        public string WebHookToken  { get; set; }
        public string? BotName  { get; set; }
        public string? AvatarURL  { get; set; }
        public string? PrefixFilter  { get; set; }
        public string? SuffixFilter  { get; set; }
        public string? RegexFilter  { get; set; }
        public bool FilterExceptions  { get; set; }
        public IFormatProvider? FormatProvider  { get; set; }
        public LogEventLevel MinimumLogEventLevel  { get; set; }
    }
}