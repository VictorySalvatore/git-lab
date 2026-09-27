using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructPatterns
{
    public interface Device
    {
        void Print(string data);
    }
    public class Monitor : Device
    {
        public void Print(string data)
        {
            Console.WriteLine("Displaying on monitor: " + data);
        }
    }
    public class Printer : Device
    {
        public void Print(string data)
        {
            Console.WriteLine("Printing to paper: " + data);
        }
    }
    public abstract class Output // базовый класс абстракции
    {
        protected readonly Device Device;
        protected Output(Device device)
        {
            Device = device;
        }
        public abstract void Render(string data);
    }
    public class TextOutput : Output
    {
        public TextOutput(Device device) : base(device) { }
        public override void Render(string data)
        {
            Device.Print("Text: " + data);
        }
    }
    public class ImageOutput : Output
    {
        public ImageOutput(Device device) : base(device) { }
        public override void Render(string data)
        {
            Device.Print("Image: [Binary data: " + data + "]");
        }
    }
}
