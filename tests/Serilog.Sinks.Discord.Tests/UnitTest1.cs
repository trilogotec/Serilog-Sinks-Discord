using NUnit.Framework;
using Serilog.Events;
using Serilog.Parsing;

namespace Serilog.Formatting.Discord.Tests;

public class Tests
{
    private static LogEvent CreateLogEvent(LogEventLevel level = Events.LogEventLevel.Information, string messageTemplate = "Hello from Serilog", Exception? exception = null, params LogEventProperty[] properties)
    {
        return new LogEvent(
            new DateTimeOffset(2003, 1, 4, 15, 9, 26, 535, TimeSpan.FromHours(1)),
            level,
            exception,
            new MessageTemplateParser().Parse(messageTemplate),
            properties
        );
    }

    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}