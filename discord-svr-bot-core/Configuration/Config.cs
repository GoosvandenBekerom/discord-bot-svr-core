using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace discord_svr_bot_core.Configuration
{
    public class Config
    {
        private readonly JObject _config;

        public Config()
        {
            var execPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            using (StreamReader file = File.OpenText(Path.Combine(execPath, "config.json")))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                _config = (JObject) JToken.ReadFrom(reader);
            }
        }
        
        public T Get<T>(string key)
        {
            return _config.GetValue(key).ToObject<T>();
        }
    }
}