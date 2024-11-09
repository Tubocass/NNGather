using UnityEngine;
using UnityEngine.UI;

namespace Gather.UI.Canvas
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Image image;
        MaxCounter counter;

        public void SetCounter(MaxCounter counter)
        {
            this.counter = counter;
            counter.counterEvent.AddListener(UpdateHealth);
        }

        void UpdateHealth()
        {
            image.fillAmount = Mathf.InverseLerp(0, counter.GetMax(), counter.GetAmount());
        }

    }
}