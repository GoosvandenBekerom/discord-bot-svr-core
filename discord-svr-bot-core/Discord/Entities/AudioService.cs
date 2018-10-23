using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using discord_svr_bot_core.Logging;
using Discord;
using Discord.Audio;

namespace discord_svr_bot_core.Discord.Entities
{
    public class AudioService
    {
        private readonly ConcurrentDictionary<ulong, IAudioClient> ConnectedChannels
            = new ConcurrentDictionary<ulong, IAudioClient>();

        public async Task JoinAudio(IGuild guild, IVoiceChannel target)
        {
            if (ConnectedChannels.TryGetValue(guild.Id, out _)) return;
            if (target.Guild.Id != guild.Id) return;

            var audioClient = await target.ConnectAsync();

            if (ConnectedChannels.TryAdd(guild.Id, audioClient))
            {
                DI.Resolve<ILogger>().Log($"Connected to voice on {guild.Name}.");
            }
        }

        public async Task LeaveAudio(IGuild guild)
        {
            if (ConnectedChannels.TryRemove(guild.Id, out var client))
            {
                await client.StopAsync();
                DI.Resolve<ILogger>().Log($"Disconnected from voice on {guild.Name}.");
            }
        }

        public async Task SendAudioAsync(IGuild guild, IMessageChannel channel, string path)
        {
            if (!File.Exists(path))
            {
                await channel.SendMessageAsync("Sorry paaz, maar daar ging iets mis.");
                return;
            }

            if (ConnectedChannels.TryGetValue(guild.Id, out var client))
            {
                var cts = new CancellationTokenSource();
                var logger = DI.Resolve<ILogger>();
                using (var ffmpeg = CreateProcess(path))
                using (var stream = client.CreatePCMStream(AudioApplication.Voice))
                {
                    try
                    {
                        logger.Log($"Starting playback of {path} in {guild.Name}");
                        await ffmpeg.StandardOutput.BaseStream.CopyToAsync(stream, CancellationToken.None);
                        cts.CancelAfter(2000); // Cancel flush if it hangs for 2 seconds
                        await stream.FlushAsync(cts.Token);
                    }
                    catch (Exception)
                    {
                        logger.Warn("Streaming audio canceled");
                        await channel.SendMessageAsync("Ik ben een Paaz, want dat kan ik niet uitspreken.");
                    }
                }
            }
        }

        private static Process CreateProcess(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel error -i {path} -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
        }
    }
}