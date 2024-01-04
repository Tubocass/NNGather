using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace gather
{
    [CreateAssetMenu]
    public class Counter : ScriptableObject
    {
        public UnityEvent counterEvent = new UnityEvent();
        public int defaultAmount;
        private int amount = 0;

        public int Amount { 
            get => amount; 
            set { amount = value; counterEvent?.Invoke(); } 
        }

        public void AddAmount(int value)
        {
            Amount += value;
            counterEvent?.Invoke();
        }

        protected virtual void OnEnable()
        {
            Amount = defaultAmount;
        }
    }
}