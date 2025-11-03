# Serilog sink for Discord

### Write your logs to discord.

![Serilog](/Screenshots/screenshot.png?raw=true)

![Serilog](/Screenshots/screenshot1.png?raw=true)

# To get started
Get ```WebhookId``` and ```WebhookToken``` </br>
> [Create webhoook](https://support.discord.com/hc/en-us/articles/228383668-Intro-to-Webhooks) and copy its url
which contains WebhookId and WebhookToken: </br>
```https://discordapp.com/api/webhooks/{WebhookId}/{WebhookToken}```

# Installation
```
Install-Package Trilogo.Serilog.Sinks.Discord
```
Note that a Discord Webhook is required in order to configure the sink, if you don't know how the create a Webhook please [refer to the official documentation](https://support.discord.com/hc/en-us/articles/228383668-Intro-to-Webhooks).


# Configuration

#### Available Log Levels
-   Verbose
-   Debug
-   Information
-   Warning
-   Error
#### In Code Configuration
To configure the Serilog sink for Discord, add the following section to your LoggerConfiguration:
```csharp
Log.Logger = new  LoggerConfiguration()
	.WriteTo.Discord({WebhookId}, {WebhookToken}, {LogEventLevel.LogLevel}, {botName}, {avatarURL})
	.CreateLogger();
```

for async logging you can use [serilog-sinks-async](https://github.com/serilog/serilog-sinks-async)
```C#
Log.Logger = new LoggerConfiguration()
 .WriteTo.Async( a => 
     a.Discord({WebhookId}, {WebhookToken}, {LogEventLevel.LogLevel}, {botName}, {avatarURL}))
 .Enrich.FromLogContext()
 .CreateLogger();
```

#### Optoinal fields
- **LogLevel**: Must be specified in the format "LogEventLevel.Debug/Warning/etc." Defaults to "Debug" if not provided.
- **botName**: If not in use, set to "null". Note: If `avatarURL` is set, `botName` is required.
- **avatarURL**
#### App Settings Configuration
To configure the Serilog sink for Discord, add the following section to your appsettings.json:
```json
{
  "Serilog": {
    "Using": [
      "Serilog.Sinks.Discord"
    ],
    "WriteTo": [
      {
        "Name": "Discord",
        "Args": {
          "webhookId": "YOUR_DISCORD_WEBHOOK_ID",
          "webhookToken": "YOUR_DISCORD_WEBHOOK_TOKEN",
          "botName": "OPTIONAL_BOT_NAME",
          "avatarURL": "OPTIONAL_AVATAR_URL",
          "minimumLevel": "LOG_LEVEL"
        }
      }
    ]
  }
}
```
#### Configuration Details:
- **webhookId**: The ID of the Discord webhook. (Required)
- **webhookToken**: The token of the Discord webhook. (Required)
- **botName**: The name under which the log messages will be sent. If left empty or not provided, no name will be displayed.
- **avatarURL**: URL pointing to an avatar image for the bot. If left empty or not provided, no avatar will be displayed.
- **minimumLevel**: Minimum log level required for messages to be sent to Discord. (e.g., "Debug", "Information")

Note: Remember to replace placeholders (YOUR_DISCORD_WEBHOOK_ID, YOUR_DISCORD_WEBHOOK_TOKEN, etc.) with actual values when integrating this into your project.