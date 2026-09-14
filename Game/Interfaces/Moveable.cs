using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Game.GameObjects;

namespace Game.Interfaces
{
    public interface Moveable
    {
        void Move(int dx, int dy);
    }
}
