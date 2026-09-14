using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Game.GameObjects;

namespace Game.Interfaces
{
    public interface Attacker
    {
        void Attack(Unit unit);
    }
}
