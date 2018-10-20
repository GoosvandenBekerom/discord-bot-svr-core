using Discord.WebSocket;

namespace discord_svr_bot_core.Discord.Entities
{
    public class BotConfig
    {
        public string Token { get; set; }
        public DiscordSocketConfig SocketConfig { get; set; }
    }
}