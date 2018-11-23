using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using FOAASClient;

namespace discord_svr_bot_core.Discord.Entities.Commands
{
    public class KCommands : ModuleBase<ICommandContext>
    {
       
        private static readonly HttpClient _client = new HttpClient();

        public KCommands()
        {


        }

        [Command("k", RunMode = RunMode.Async), Summary("Fuck off, K")]
        public async Task K()
        {
            try
            {
                var foaasClient = new FOAASClient.FoaasClient();
                List<MethodInfo> allMethods = foaasClient.GetType().GetMethods().ToList()
                    .Where(w => w.GetParameters().Length > 0 && w.GetParameters().Any(a => a.Name == "name")).ToList();
                var selectedMethod = allMethods[new Random().Next(0, allMethods.Count)];
                var methodParameters = selectedMethod.GetParameters();
                object[] paramList = new object[methodParameters.Length];
                var cnt = 0;
                foreach (var p in methodParameters)
                {
                    switch (p.Name)
                    {
                        case "name":
                            paramList[cnt] = "K";
                            break;
                        default:
                            paramList[cnt] = "";
                            break;
                    }
                    cnt++;
                }
                var result = await (Task<Response>)selectedMethod.Invoke(foaasClient, paramList);
                await Context.Channel.SendMessageAsync(result.Message.ToString());
            }
            catch (Exception ex)
            {
                await Context.Channel.SendMessageAsync("Error 43: K stinkt naar poep.");
            }
        }


 
    }
}
