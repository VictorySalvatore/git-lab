using System;
using Game.GameObjects;

namespace Game
{
    internal static class Program
    {
        private static void Main()
        {
            Archer dean = new Archer("Dean", 0, 0, 100, 25, 5);
            Archer sam = new Archer("Sam", 1, 1, 80, 20, 3);
            Fort fort = new Fort("Fort", 5, 5, 2, 50, 10);
            MobileHouse cart = new MobileHouse("Cart", 2, 2, 1);

            // начальное состояние
            Console.WriteLine(dean);
            Console.WriteLine(sam);
            Console.WriteLine(fort);
            Console.WriteLine(cart);
            Console.WriteLine();

            // лучник атакует лучника
            dean.Attack(sam);
            Console.WriteLine("Sam HP: " + sam.GetHp());

            // cтроим крепость (2 хода) и атакуем
            fort.OnTurn();
            fort.OnTurn();
            fort.Attack(dean);
            Console.WriteLine("Dean HP: " + dean.GetHp());

            // лучник двигается
            dean.Move(3, 0);
            Console.WriteLine("Dean: " + dean.GetX() + ";" + dean.GetY());

            //// дом не построен и не едет
            //cart.Move(10, 10);
            //Console.WriteLine("Cart: " + cart.GetX() + ";" + cart.GetY());

            // строим дом (1 ход) и он едет
            cart.OnTurn();
            cart.Move(10, 10);
            Console.WriteLine("Cart: " + cart.GetX() + ";" + cart.GetY());

            // убиваем лучника
            dean.ReceiveDamage(200);
            Console.WriteLine("Dean жив? " + dean.IsAlive());
            dean.Move(1, 1);   // мёртвый не двинется
            Console.WriteLine("Dean: " + dean.GetX() + ";" + dean.GetY());
        }
    }
}
