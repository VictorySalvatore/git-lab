using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Game.Interfaces;

namespace Game.GameObjects
{
    public class Fort : Building, Attacker
    {
        private readonly float dam;
        private readonly int attackR;

        public Fort(string name, int x, int y, int buildTimeTurns,
                    float damage, int attackRange, bool built = false)
            : base(name, x, y, buildTimeTurns, built)
        {
            dam = damage;
            attackR = attackRange;
        }

        public void Attack(Unit unit)
        {
            if (unit is null)
                throw new ArgumentNullException(nameof(unit));

            if (!IsBuilt()) return;      // недостроенная крепость не стреляет
            if (!unit.IsAlive()) return;

            int dist = Math.Abs(GetX() - unit.GetX())
                     + Math.Abs(GetY() - unit.GetY());
            if (dist > attackR) return;

            unit.ReceiveDamage(dam);
        }
    }
}
