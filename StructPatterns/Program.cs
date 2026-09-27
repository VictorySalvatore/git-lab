namespace StructPatterns
{
    internal class Program
    {
        public static void Main()
        {
            RunProxy();
            Console.WriteLine();

            RunAdapter();
            Console.WriteLine();

            RunBridge();
        }
        private static void RunProxy()
        {
            Console.WriteLine("Proxy");
            Database userDb = new DatabaseProxy(false);
            Database adminDb = new DatabaseProxy(true);
            userDb.Query("SELECT * FROM users");
            adminDb.Query("SELECT * FROM users");
        }
        private static void RunAdapter()
        {
            Console.WriteLine("Adapter");
            ExternalLogger externalLogger = new ExternalLogger();
            Logger logger = new LoggerAdapter(externalLogger);
            logger.Log("Hello, world!");
        }
        private static void RunBridge()
        {
            Console.WriteLine("Bridge");
            Device monitor = new Monitor();
            Device printer = new Printer();
            Output textOnMonitor = new TextOutput(monitor);
            Output textOnPrinter = new TextOutput(printer);
            textOnMonitor.Render("Hello, world!");
            textOnPrinter.Render("Hello, world!");
            Output imageOnMonitor = new ImageOutput(monitor);
            imageOnMonitor.Render("101010101");
        }
    }
}