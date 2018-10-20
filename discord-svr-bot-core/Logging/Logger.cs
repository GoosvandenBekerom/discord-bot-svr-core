using System;
using System.IO;
using System.Reflection;

namespace discord_svr_bot_core.Logging
{
    public class Logger : ILogger
    {
        private readonly string _logFolder;

        public Logger()
        {
            var executionPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            _logFolder = Path.Combine(executionPath, "log");
            Directory.CreateDirectory(_logFolder);
        }

        public void Log(string message, ConsoleColor color = ConsoleColor.White)
        {
            ProcessLog("info", message, color, false);
        }

        public void Warn(string message, ConsoleColor color = ConsoleColor.Yellow)
        {
            ProcessLog("warning", message, color);
        }

        public void Error(string message, ConsoleColor color = ConsoleColor.Red)
        {
            ProcessLog("error", message, color);
        }

        private void ProcessLog(string severity, string message, ConsoleColor color, bool writeToFile = true)
        {
            string formatted = $"{DateTime.Now.ToLongTimeString()} - [{severity.ToUpper()}]\t: {message}";
            Console.ForegroundColor = color;
            Console.WriteLine(formatted);
            if (writeToFile) WriteToFile(formatted);
        }

        private void WriteToFile(string message)
        {
            var filename = $"{DateTime.Today.ToShortDateString()}.log";
            var path = Path.Combine(_logFolder, filename);
            using (StreamWriter file = new StreamWriter(path, true))
            {
                file.WriteLine(message);
            }
        }
    }
}