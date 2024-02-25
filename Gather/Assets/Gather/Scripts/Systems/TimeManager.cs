using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

namespace gather
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] Transform lightTransform;
        [SerializeField] Light2D sun;
        [SerializeField] Light2D moon;
        [SerializeField] Light2D ambient;

        [SerializeField] Gradient sunGradient;
        [SerializeField] Gradient moonGradient;
        [SerializeField] Gradient ambientGradient;

        [SerializeField] int lengthOfDay;
        [SerializeField] float timeOfDay;
        [SerializeField] float timeOfDawn;
        [SerializeField] float timeOfDusk;

        public float dayRatio => timeOfDay / lengthOfDay;

        public UnityEvent OnDawn;
        public UnityEvent OnDusk;

        private void Start()
        {
            lightTransform = transform;
            OnDawn.AddListener(AtDawn);
            OnDusk.AddListener(AtDusk);
        }

        private void Update()
        {
            UpdateLight();
            float previousRatio = dayRatio;
            timeOfDay += Time.deltaTime;
            if (timeOfDay > lengthOfDay)
            {
                timeOfDay -= lengthOfDay;
            }

            bool prev = IsInRange(previousRatio);
            bool current = IsInRange(dayRatio);

            if(prev && !current)
            {
                OnDusk?.Invoke();
            }else if(!prev && current)
            {
                OnDawn?.Invoke();
            }
        }

        bool IsInRange(float t)
        {
            return t>= timeOfDawn/lengthOfDay && t<= timeOfDusk/lengthOfDay;
        }

        public bool IsDaylight()
        {
            return IsInRange(dayRatio);
        }

        private void UpdateLight()
        {
            float ratio = timeOfDay / lengthOfDay;
            sun.color = sunGradient.Evaluate(ratio);
            moon.color = moonGradient.Evaluate(ratio);
            ambient.color = ambientGradient.Evaluate(ratio);

            lightTransform.rotation = Quaternion.Euler(0, 0, 360.0f * ratio);
        }

        public void AtDawn()
        {
            Debug.Log("Dawn");
        }

        public void AtDusk()
        {
            Debug.Log("Dusk");
        }
    }
}