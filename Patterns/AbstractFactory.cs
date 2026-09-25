using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.AbstractFactory // создаёт семейство связанных продуктов через композицию
{
    public interface Button
    {
        void Paint();
    }
    public interface Checkbox
    {
        void Paint();
    }
    public class WindowsButton : Button // Конкретные продукты для Windows
    {
        public void Paint() => Console.WriteLine("You have created a Windows button.");
    }
    public class WindowsCheckbox : Checkbox
    {
        public void Paint() => Console.WriteLine("You have created a Windows checkbox.");
    }
    public class MacButton : Button // Конкретные продукты для Mac
    {
        public void Paint() => Console.WriteLine("You have created a Mac button.");
    }
    public class MacCheckbox : Checkbox
    {
        public void Paint() => Console.WriteLine("You have created a Mac checkbox.");
    }
    public interface GUIFactory
    {
        Button CreateButton();
        Checkbox CreateCheckbox();
    }
    public class WindowsFactory : GUIFactory // Конкретные фабрики
    {
        public Button CreateButton() => new WindowsButton();
        public Checkbox CreateCheckbox() => new WindowsCheckbox();
    }
    public class MacFactory : GUIFactory
    {
        public Button CreateButton() => new MacButton();
        public Checkbox CreateCheckbox() => new MacCheckbox();
    }
    public class Application // Клиентский код
    {
        private readonly Button button;
        private readonly Checkbox checkbox;

        public Application(GUIFactory factory)
        {
            button = factory.CreateButton();
            checkbox = factory.CreateCheckbox();
        }
        public void Paint()
        {
            button.Paint();
            checkbox.Paint();
        }
    }
}
