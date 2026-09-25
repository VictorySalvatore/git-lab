using Patterns.AbstractFactory;
using Patterns.Builder;
using Patterns.FactoryMethod;
using Patterns.Singleton;

namespace Patterns
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1. Singleton");
            var config1 = GameConfig.Instance; // ссылки на память
            var config2 = GameConfig.Instance;
            Console.WriteLine("Один и тот же объект? " + object.ReferenceEquals(config1, config2));
            config1.Print();

            Console.WriteLine("\n2. Factory Method");
            LoggerFactory f1 = new FileLoggerFactory();
            f1.LogMessage("Файловая фабрика");
            LoggerFactory f2 = new ConsoleLoggerFactory();
            f2.LogMessage("Консольная фабрика");

            Console.WriteLine("\n3. Abstract Factory");
            new Application(new WindowsFactory()).Paint();
            new Application(new MacFactory()).Paint();

            Console.WriteLine("\n4. Builder");
            var hb = new HawaiianPizzaBuilder();
            new PizzaDirector(hb).ConstructPizza();
            Console.WriteLine("Гавайская: " + hb.GetResult());

            var pb = new PepperoniPizzaBuilder();
            new PizzaDirector(pb).ConstructPizza();
            Console.WriteLine("Пепперони: " + pb.GetResult());
        }
    }
}
