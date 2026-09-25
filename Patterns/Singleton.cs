using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Singleton
{
    public class GameConfig
    {
        private static GameConfig instance; // поле с ТЕМ САМЫМ экземпляром
        private GameConfig() // приватный конструктор
        {
            Diff = "Normal";
            Sound = 50;
        }
        public static GameConfig Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameConfig();
                return instance;
            }
        }
        public string Diff { get; set; }
        public int Sound { get; set; }
        public void Print()
        {
            Console.WriteLine($"Difficulty: {Diff}, Volume: {Sound}");
        }
    }
}
