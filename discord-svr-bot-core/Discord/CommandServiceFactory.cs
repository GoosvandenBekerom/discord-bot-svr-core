using System.Reflection;
using Discord.Commands;

namespace discord_svr_bot_core.Discord
{
    public static class CommandServiceFactory
    {
        public static CommandService Default => new CommandService(new CommandServiceConfig
        {
            CaseSensitiveCommands = false
        });
    }
}