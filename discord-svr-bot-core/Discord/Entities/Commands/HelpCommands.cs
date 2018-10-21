using System.Linq;
using System.Threading.Tasks;
using discord_svr_bot_core.Config;
using Discord;
using Discord.Commands;

namespace discord_svr_bot_core.Discord.Entities.Commands
{
    public class HelpCommands : ModuleBase
    {
        private readonly CommandService _service;

        public HelpCommands(CommandService service)
        {
            _service = service;
        }

        [Command("help"), Summary("Display all available commands")]
        public async Task HelpAsync()
        {
            string prefix = ConfigStore.Get<string>("prefix");
            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Description = "Dit zijn al je commands, Paaz."
            };

            foreach (var module in _service.Modules)
            {
                string description = null;
                foreach (var cmd in module.Commands)
                {
                    var result = await cmd.CheckPreconditionsAsync(Context);
                    if (!result.IsSuccess) continue;

                    var parameters = cmd.Parameters.Count > 0
                        ? $"[{string.Join(", ", cmd.Parameters.Select(p => p.Name))}]"
                        : string.Empty;

                    var summary = !string.IsNullOrEmpty(cmd.Summary)
                        ? $"- {cmd.Summary}"
                        : string.Empty;
                    
                    description += $"{prefix}{cmd.Aliases.First()} {parameters} {summary}\n";
                }

                if (!string.IsNullOrWhiteSpace(description))
                {
                    builder.AddField(x =>
                    {
                        x.Name = module.Name;
                        x.Value = description;
                        x.IsInline = false;
                    });
                }
            }

            await ReplyAsync("", false, builder.Build());
        }

        [Command("help"), Summary("Display detailed command")]
        public async Task HelpAsync(string command)
        {
            var result = _service.Search(Context, command);

            if (!result.IsSuccess)
            {
                await ReplyAsync($"Sorry, vuile paaz. **{command}** bestaat niet (net als Miel Stroop).");
                return;
            }
            
            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Description = $"Here are some commands like **{command}**"
            };

            foreach (var match in result.Commands)
            {
                var cmd = match.Command;

                builder.AddField(x =>
                {
                    x.Name = string.Join(", ", cmd.Aliases);
                    x.Value = $"Parameters: {string.Join(", ", cmd.Parameters.Select(p => p.Name))}\n" +
                              $"Summary: {cmd.Summary}";
                    x.IsInline = false;
                });
            }

            await ReplyAsync("", false, builder.Build());
        }
    }
}