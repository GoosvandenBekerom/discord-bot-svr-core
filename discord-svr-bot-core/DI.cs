using discord_svr_bot_core.Discord;
using discord_svr_bot_core.Logging;
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
            _container.RegisterSingleton<ILogger, Logger>();
            _container.RegisterType<DiscordSocketConfig>(new InjectionFactory(i => SocketConfigFactory.Default));
            _container.RegisterSingleton<DiscordSocketClient>(new InjectionConstructor(typeof(DiscordSocketConfig)));
        }

        public static T Resolve<T>()
        {
            return (T) Container.Resolve(typeof(T), string.Empty, new CompositeResolverOverride());
        }
    }
}