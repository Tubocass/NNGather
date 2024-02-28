using UnityEngine;

namespace Gather
{
    [System.Serializable]
    public class MaxCounter : Counter
    {
        private int max;

        public int GetMax() { return this.max; }

        public void SetMax(int max) { this.max = max; }

        public override void SetAmount(int value)
        {
            base.SetAmount(Mathf.Clamp(value, 0, max));
        }
    }
}