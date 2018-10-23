using System;
using System.Threading.Tasks;

namespace discord_svr_bot_core.Speech
{
    public interface ISpeechProvider
    {
        Task<string> ObtainSpeechPathAsync(string text, string language = "nl-NL", bool male = true);
    }
}