using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Game.Interfaces;

namespace Game.GameObjects
{
    public class MobileHouse : Building, Moveable
    {
        public MobileHouse(string name, int x, int y, int buildTimeTurns, bool built = false)
            : base(name, x, y, buildTimeTurns, built)
        {
        }

        public void Move(int dx, int dy)
        {
            if (!IsBuilt()) return;   // недостроенный дом не едет
            X += dx;
            Y += dy;
        }
    }
}