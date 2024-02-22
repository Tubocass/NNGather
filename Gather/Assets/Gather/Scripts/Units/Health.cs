using UnityEngine;

namespace gather
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHP = 2;
        [SerializeField] int hp;
        Unit unit;

        private void Start()
        {
            unit = GetComponent<Unit>();
            hp = maxHP;
        }

        public void TakeDamage(int amount)
        {
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
    }
}