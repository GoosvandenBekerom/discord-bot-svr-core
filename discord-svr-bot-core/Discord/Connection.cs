using System;
using System.Threading.Tasks;
using discord_svr_bot_core.Discord.Entities;
using discord_svr_bot_core.Logging;
using Discord;
using Discord.WebSocket;

namespace discord_svr_bot_core.Discord
{
    public class Connection
    {
        private readonly DiscordSocketClient _client;
        private readonly Logger _logger;

        public Connection(DiscordSocketClient client, Logger logger)
        {
            _client = client;
            _logger = logger;
        }

        internal async Task ConnectAsync(BotConfig config)
        {
            _client.Log += OnDiscordLogMessage;

            await _client.LoginAsync(TokenType.Bot, config.Token);
            await _client.StartAsync();


        }

        private Task OnDiscordLogMessage(LogMessage message)
        {
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                    return Task.Run(() => _logger.Error(message.Message));
                case LogSeverity.Error:
                    return Task.Run(() => _logger.Error(message.Message));
                case LogSeverity.Warning:
                    return Task.Run(() => _logger.Warn(message.Message));
                default:
                    return Task.Run(() => _logger.Log(message.Message));
            }
        }
    }
}