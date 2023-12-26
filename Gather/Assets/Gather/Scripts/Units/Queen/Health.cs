using gather;
using System;
using UnityEngine;

namespace gather
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHP = 10;
        int hp;
        Unit unit;

        public void TakeDamage(int amount)
        {
            if(hp <= 0)
            {
                return;
            }

            hp -= amount;
            if (hp <= 0)
            {
                unit.Death();
            }
        }

        public void Heal(int amount)
        {
            if(hp < maxHP)
            {
                hp += amount;
            }
        }

        private void Start()
        {
            unit = GetComponent<Unit>();
            hp = maxHP;
        }
    }
}