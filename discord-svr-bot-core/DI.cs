using discord_svr_bot_core.Configuration;
using discord_svr_bot_core.Discord;
using discord_svr_bot_core.Discord.Entities;
using discord_svr_bot_core.Logging;
using discord_svr_bot_core.Speech;
using Discord.Commands;
using Discord.WebSocket;
using Unity;
using Unity.Injection;
using Unity.Resolution;

namespace discord_svr_bot_core
{
    public static class DI
    {
        private static UnityContainer _container;

        public static UnityContainer Container
        {
            get
            {
                if (_container == null)
                    RegisterTypes();
                return _container;
            }
        }

        public static void RegisterTypes()
        {
            _container = new UnityContainer();

            // Config
            _container.RegisterSingleton<Config>();

            // Logging
            _container.RegisterSingleton<ILogger, Logger>();

            // Discord
            _container.RegisterType<DiscordSocketConfig>(new InjectionFactory(i => SocketConfigFactory.Default));
            _container.RegisterSingleton<DiscordSocketClient>(new InjectionConstructor(typeof(DiscordSocketConfig)));
            _container.RegisterSingleton<CommandService>(new InjectionFactory(i => CommandServiceFactory.Default));
            _container.RegisterSingleton<AudioService>();

            // Text To Speech
            _container.RegisterSingleton<ISpeechProvider, GoogleCloudSpeechProvider>();
        }

        public static T Resolve<T>()
        {
            return (T) Container.Resolve(typeof(T), string.Empty, new CompositeResolverOverride());
        }
    }
}