using System.Threading.Tasks;
using discord_svr_bot_core.Logging;
using Discord.Commands;

namespace discord_svr_bot_core.Discord.Entities.Commands
{
    public class TextToSpeechCommands : ModuleBase
    {
        [Command("say"), Summary("Speaks out message in dutch")]
        public async Task Say([Remainder, Summary("The message to speak out")] string message)
        {
            var logger = DI.Resolve<ILogger>();
            logger.Log($"Execute Say command: {message}");
            
            await ReplyAsync(message);
        }
    }
}