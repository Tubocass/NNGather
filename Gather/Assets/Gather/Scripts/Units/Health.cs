using UnityEngine;

namespace Gather
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int maxHP;
        MaxCounter hp;
        Unit unit;

        private void Awake()
        {
            unit = GetComponent<Unit>();
            hp = ScriptableObject.CreateInstance<MaxCounter>();
            hp.SetMax(maxHP);
            hp.SetAmount(maxHP);
        }

        public MaxCounter GetCounter()
        {
            return hp;
        }

        public void TakeDamage(int amount)
        {
            hp.AddAmount(-amount);
            if (hp.GetAmount() <= 0)
            {
                unit.Death();
            }
        }

        public void Heal(int amount)
        {
            hp.AddAmount(amount);
        }
    }
}