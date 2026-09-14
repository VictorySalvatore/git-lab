using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.GameObjects
{
    public class Building : GameObject // достроена или нет
    {
        private bool b;
        private readonly int buildTime; // за сколько ходов строится
        private int turnsSpent; // потраченные ходы

        public Building(string name, int x, int y, int buildTimeTurns, bool built = false)
            : base(name, x, y)
        {
            buildTime = buildTimeTurns;
            b = built;
            turnsSpent = built ? buildTimeTurns : 0;
        }

        public bool IsBuilt() => b;

        public virtual void OnTurn()
        {
            if (b) return;

            turnsSpent++;
            if (turnsSpent >= buildTime)
                b = true;
        }
    }
}
