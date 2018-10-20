using System;

namespace discord_svr_bot_core.Logging
{
    internal interface ILogger
    {
        void Log(string message, ConsoleColor color = ConsoleColor.White);
        void Warn(string message, ConsoleColor color = ConsoleColor.Yellow);
        void Error(string message, ConsoleColor color = ConsoleColor.Red);
    }
}
