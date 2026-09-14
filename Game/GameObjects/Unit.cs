using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.GameObjects
{
    public class Unit : GameObject
    {
        private float hp;
        private readonly float maxHp;

        public Unit(string name, int x, int y, float mxHp)
            : base(name, x, y) // вызывает конструктор родителя
        {
            if (mxHp <= 0)
                throw new ArgumentOutOfRangeException(nameof(mxHp), "Максимальное HP больше 0");

            maxHp = mxHp;
            hp = maxHp;
        }

        public bool IsAlive() => hp > 0;

        public float GetHp() => hp;

        public virtual void ReceiveDamage(float damage)
        {
            if (damage < 0)
                throw new ArgumentOutOfRangeException(nameof(damage), "Урон больше 0");

            if (!IsAlive()) return;   // мёртвого не бьём

            hp -= damage;
            if (hp < 0) hp = 0;
        }
    }
}
