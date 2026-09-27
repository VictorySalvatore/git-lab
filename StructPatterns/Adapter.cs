using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructPatterns
{
    public class ExternalLogger
    {
        public void LogMessage(string msg)
        {
            Console.WriteLine("External log: " + msg);
        }
    }
    public interface Logger // Target — ожидаемый интерфейс
    {
        void Log(string message);
    }
    public class LoggerAdapter : Logger
    {
        private readonly ExternalLogger externalLog;

        public LoggerAdapter(ExternalLogger externalLogger)
        {
            externalLog = externalLogger;
        }
        public void Log(string message)
        {
            externalLog.LogMessage(message);
        }
    }
}
