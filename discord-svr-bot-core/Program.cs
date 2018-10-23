using System;
using System.Threading;
using System.Threading.Tasks;
using discord_svr_bot_core.Configuration;
using discord_svr_bot_core.Discord;
using discord_svr_bot_core.Discord.Entities;

namespace discord_svr_bot_core
{
    internal class Program
    {
        public static CancellationTokenSource EndOfApplication;

        private static async Task Main(string[] args)
        {
            DI.RegisterTypes();
            EndOfApplication = new CancellationTokenSource();
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            
            var connection = DI.Resolve<Connection>();
            await connection.ConnectAsync(new BotConfig
            {
                Token = DI.Resolve<Config>().Get<string>("token")
            });
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            EndOfApplication.Cancel();
        }
    }
}
