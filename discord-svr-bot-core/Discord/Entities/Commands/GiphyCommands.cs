using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using discord_svr_bot_core.Configuration;
using discord_svr_bot_core.Models.Giphy;
using Discord.Commands;
using Newtonsoft.Json;

namespace discord_svr_bot_core.Discord.Entities.Commands
{
    public class GiphyCommands : ModuleBase<ICommandContext>
    {
        private readonly string _key;
        private static readonly HttpClient _client = new HttpClient();
        private static readonly int _limit = 100;
        public GiphyCommands()
        {
            _key = DI.Resolve<Config>().Get<string>("giphy");
        }

        [Command("rgif", RunMode = RunMode.Async), Summary("Get a random .gif from giphy")]
        public async Task RandomGif([Remainder, Summary("Keyword to search for")] string tag)
        { 
            var url = $"https://api.giphy.com/v1/gifs/random?api_key={_key}&rating=R&tag={tag}";
            try
            {
                var obj = JsonConvert.DeserializeObject<GiphyModel.RootObject2>(await _client.GetStringAsync(url));
                await Context.Channel.SendMessageAsync(obj != null ? obj.data?.url : "Gif niet gevonden, PaaZ");
            }
            catch (Exception ex)
            {
                await Context.Channel.SendMessageAsync("Gif niet gevonden, PaaZ");
            }
        }


        [Command("gif", RunMode = RunMode.Async), Summary("Search a .gif from giphy")]
        public async Task Gif2([Remainder, Summary("Keyword to search for")] string query)
        {
            var url = $"https://api.giphy.com/v1/gifs/search?api_key={_key}&q={query}&limit={_limit}&lang=nl";
            var obj = JsonConvert.DeserializeObject<GiphyModel.RootObject>(await _client.GetStringAsync(url));
            await Context.Channel.SendMessageAsync(obj.data.FirstOrDefault()?.url ?? "Gif niet gevonden, PaaZ");
        }
    }

}