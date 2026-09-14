using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Game.Interfaces;

namespace Game.GameObjects
{
    public class Archer : Unit, Attacker, Moveable
    {
        private readonly float dam;
        private readonly int attackR; // дистанция

        public Archer(string name, int x, int y, float maxHp, float damage, int attackRange)
            : base(name, x, y, maxHp)
        {
            dam = damage;
            attackR = attackRange;
        }

        public void Attack(Unit unit)
        {
            if (unit is null)
                throw new System.ArgumentNullException(nameof(unit));

            if (!IsAlive()) return;
            if (!unit.IsAlive()) return;

            // проверяем дистанцию
            int dist = System.Math.Abs(GetX() - unit.GetX())
                     + System.Math.Abs(GetY() - unit.GetY());
            if (dist > attackR) return;

            unit.ReceiveDamage(dam);
        }

        public void Move(int dx, int dy)
        {
            if (!IsAlive()) return;   // мёртвый не ходит
            X += dx;
            Y += dy;
        }
    }
}
