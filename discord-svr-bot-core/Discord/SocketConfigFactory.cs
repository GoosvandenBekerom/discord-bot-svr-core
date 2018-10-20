using Discord;
using Discord.WebSocket;

namespace discord_svr_bot_core.Discord
{
    public static class SocketConfigFactory
    {
        public static DiscordSocketConfig Default => new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Verbose,
        };

        public static DiscordSocketConfig Generate => new DiscordSocketConfig();
    }
}