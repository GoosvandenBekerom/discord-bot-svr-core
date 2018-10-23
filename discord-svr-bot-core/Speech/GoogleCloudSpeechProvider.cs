using System;
using System.IO;
using System.Threading.Tasks;
using discord_svr_bot_core.Logging;
using Google.Cloud.TextToSpeech.V1;

namespace discord_svr_bot_core.Speech
{
    public class GoogleCloudSpeechProvider : ISpeechProvider
    {
        private readonly ILogger _logger;

        public GoogleCloudSpeechProvider(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<string> ObtainSpeechPathAsync(string text, string language = "en-US", bool male = true)
        {
            TextToSpeechClient client = TextToSpeechClient.Create();

            var response = await client.SynthesizeSpeechAsync(new SynthesizeSpeechRequest
            {
                Input = new SynthesisInput
                {
                    Text = text
                },
                Voice = new VoiceSelectionParams
                {
                    LanguageCode = language,
                    SsmlGender = male ? SsmlVoiceGender.Male : SsmlVoiceGender.Female
                },
                AudioConfig = new AudioConfig
                {
                    AudioEncoding = AudioEncoding.Mp3
                }
            });

            var filename = Path.GetTempFileName();
            using (Stream output = File.Create(filename))
            {
                response.AudioContent.WriteTo(output);
                _logger.Log($"Audio content written to temp file '{filename}'");
            }

            return filename;
        }
    }
}