using UnityEngine;

namespace gather
{
    [CreateAssetMenu]
    public class Counter : ScriptableObject
    {
        public int defaultAmount;
        public int amount = 0;

        public void SetAmount(int value)
        {
            amount = value;
        }

        public void AddAmount(int value)
        {
            amount += value;
        }

        protected virtual void OnEnable()
        {
            amount = defaultAmount;
        }
    }
}