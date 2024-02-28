using UnityEngine;
using UnityEngine.Events;

namespace Gather
{
    [CreateAssetMenu]
    [System.Serializable]
    public class Counter : ScriptableObject
    {
        public UnityEvent counterEvent = new UnityEvent();
        protected int amount = 0;

        public int GetAmount()
        {
            return amount;
        }

        public virtual void SetAmount(int value)
        { 
            amount = value; 
            counterEvent?.Invoke(); 
        }

        public virtual void AddAmount(int value)
        {
            SetAmount(amount += value);
        }
    }
}