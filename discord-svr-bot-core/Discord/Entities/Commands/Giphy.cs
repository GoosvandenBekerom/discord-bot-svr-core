using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using discord_svr_bot_core.Configuration;
using discord_svr_bot_core.Models.Giphy;
using Discord.Commands;
using Newtonsoft.Json;

namespace discord_svr_bot_core.Discord.Entities.Commands
{
    public class Giphy : ModuleBase<ICommandContext>
    {
        private readonly string _key;
        private static readonly HttpClient client = new HttpClient();
        public Giphy()
        {
            _key = DI.Resolve<Config>().Get<string>("giphy");
        }

        [Command("giphy", RunMode = RunMode.Async), Summary("Get a random .gif from giphy")]
        public async Task Gif([Remainder, Summary("Keyword to search for")] string query)
        {
            var url = $"https://api.giphy.com/v1/gifs/search?api_key={_key}&q={query}&limit=1";
            await Context.Channel.SendMessageAsync(JsonConvert.DeserializeObject<GiphyModel.RootObject>(await client.GetStringAsync(url)).data.FirstOrDefault()?.url ?? "Gif niet gevonden, PaaZ");
        }
    }

}