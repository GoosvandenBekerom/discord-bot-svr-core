using System.Threading.Tasks;
using discord_svr_bot_core.Logging;
using discord_svr_bot_core.Speech;
using Discord;
using Discord.Commands;

namespace discord_svr_bot_core.Discord.Entities.Commands
{
    public class TextToSpeechCommands : ModuleBase<ICommandContext>
    {
        private readonly AudioService _audio;

        public TextToSpeechCommands()
        {
            _audio = DI.Resolve<AudioService>();
        }

        [Command("join", RunMode = RunMode.Async), Summary("Join current audio channel")]
        public async Task JoinChannel()
        {
            var channel = (Context.User as IVoiceState)?.VoiceChannel;
            if (channel == null)
            {
                await Context.Channel.SendMessageAsync("Vuile paaz je moet wel in voice channel zitten.");
                return;
            }

            await _audio.JoinAudio(Context.Guild, channel);
        }

        [Command("leave", RunMode = RunMode.Async), Summary("Leave current audio channel")]
        public async Task LeaveChannel()
        {
            await _audio.LeaveAudio(Context.Guild);
        }

        [Command("say", RunMode = RunMode.Async), Summary("Speaks out message in dutch")]
        public async Task Say([Remainder, Summary("The message to speak out")] string message)
        {
            var logger = DI.Resolve<ILogger>();
            var speechProvider = DI.Resolve<ISpeechProvider>();

            logger.Log($"Execute Say command: {message}");

            var speechPath = await speechProvider.ObtainSpeechPathAsync(message);

            await JoinChannel();
            await _audio.SendAudioAsync(Context.Guild, Context.Channel, speechPath);
            await ReplyAsync(message);
            await LeaveChannel();
        }
    }
}