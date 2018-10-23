using System;
using System.Reflection;
using System.Threading.Tasks;
using discord_svr_bot_core.Configuration;
using discord_svr_bot_core.Discord.Entities;
using discord_svr_bot_core.Discord.Entities.Commands;
using discord_svr_bot_core.Logging;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace discord_svr_bot_core.Discord
{
    public class Connection
    {
        private readonly DiscordSocketClient _client;
        private readonly Logger _logger;
        private readonly CommandService _commands;
        private readonly IServiceProvider _service;
        private readonly string _prefix;

        public Connection(DiscordSocketClient client, Logger logger, CommandService commands)
        {
            _client = client;
            _logger = logger;
            _commands = commands;
            _service = new ServiceCollection().BuildServiceProvider();
            _prefix = DI.Resolve<Config>().Get<string>("prefix");
        }

        internal async Task ConnectAsync(BotConfig config)
        {
            _client.Log += OnDiscordLogMessage;
            _client.MessageReceived += OnDiscordIncomingMessage;

            // Discover all commands in this assembly
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());

            await _client.LoginAsync(TokenType.Bot, config.Token);
            await _client.StartAsync();
            await _client.SetGameAsync("K is een Paaz!");
            
            await Task.Delay(-1);
        }

        private async Task OnDiscordIncomingMessage(SocketMessage msg)
        {
            if (!(msg is SocketUserMessage message)) return;

            int argPos = 0;
            if (!(message.HasStringPrefix(_prefix, ref argPos))) return;

            _logger.Log($"Received command: {message.Content}");

            var context = new CommandContext(_client, message);

            var result = await _commands.ExecuteAsync(context, argPos, _service);
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
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