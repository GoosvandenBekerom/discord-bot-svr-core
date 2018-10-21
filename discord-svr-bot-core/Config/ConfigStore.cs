using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity.Configuration.ConfigurationHelpers;

namespace discord_svr_bot_core.Config
{
    public static class ConfigStore
    {
        private static readonly Dictionary<string, object> _config = new Dictionary<string, object>
        {
            // TODO extract this to json file
            { "token", "NDA4NzY0NjE1MDM2NjMzMDk4.Dq01Ng.3QqKogMWQzvoNqUkG3fCl47Y9lQ" },
            { "prefix", "''" }
    };

        public static T Get<T>(string key)
        {
            return (T) _config.GetOrNull(key);
        }
    }
}