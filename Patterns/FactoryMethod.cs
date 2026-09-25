using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.FactoryMethod // создаёт один продукт через наследование
{
    public interface Logger
    {
        void Log(string message);
    }

    // Конкретный продукт: файловый логгер
    public class FileLogger : Logger
    {
        public void Log(string message)
        {
            Console.WriteLine("Logging to file: " + message);
        }
    }

    // Конкретный продукт: консольный логгер
    public class ConsoleLogger : Logger
    {
        public void Log(string message)
        {
            Console.WriteLine("Logging to console: " + message);
        }
    }

    // Создатель (фабрика)
    public abstract class LoggerFactory
    {
        public abstract Logger CreateLogger();
        public void LogMessage(string message)
        {
            Logger logger = CreateLogger();
            logger.Log(message);
        }
    }

    // Конкретная фабрика: файловый логгер
    public class FileLoggerFactory : LoggerFactory
    {
        public override Logger CreateLogger() => new FileLogger();
    }

    // Конкретная фабрика: консольный логгер
    public class ConsoleLoggerFactory : LoggerFactory
    {
        public override Logger CreateLogger() => new ConsoleLogger();
    }
}
