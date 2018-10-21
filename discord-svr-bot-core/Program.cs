using System.Threading;
using System.Threading.Tasks;
using discord_svr_bot_core.Config;
using discord_svr_bot_core.Discord;
using discord_svr_bot_core.Discord.Entities;

namespace discord_svr_bot_core
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            DI.RegisterTypes();
            
            var connection = DI.Resolve<Connection>();
            await connection.ConnectAsync(new BotConfig
            {
                Token = ConfigStore.Get<string>("token")
            });
        }
    }
}
