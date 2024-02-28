using UnityEngine;

namespace Gather
{
    public class MaxCounter : Counter
    {
        private int max;

        public int GetMax() { return this.max; }

        public void SetMax(int max) { this.max = max; }

        public override void SetAmount(int value)
        {
            base.SetAmount(value > max? max: value);
        }

        public override void AddAmount(int value)
        {
            base.AddAmount(value);
            Mathf.Clamp(amount, 0, max);
        }

    }
}