using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.GameObjects
{
    public abstract class GameObject
    {
        private static int count = 1;   // общий счётчик для всех

        private readonly int id;
        private readonly string name; // дается в конструкторе, дальше не меняется

        protected int X { get; set; } // доступ наследникам
        protected int Y { get; set; }

        protected GameObject(string Name, int x, int y)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("Необходимо имя", nameof(name));

            id = count++;
            name = Name;
            X = x;
            Y = y;
        }

        public int GetId() => id; // стрелочная запись

        public string GetName() => name;

        public int GetX() => X;

        public int GetY() => Y;

        public override string ToString() => // замена метода
            $"{GetType().Name}#{id} \"{name}\" ({X};{Y})";
    }
}
