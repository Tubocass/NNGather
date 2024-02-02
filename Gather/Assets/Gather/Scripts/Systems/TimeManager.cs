using UnityEngine;
using UnityEngine.Events;

namespace gather
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] Light sun;
        Transform lightTransform;
        [SerializeField] float lengthOfDay;
        [SerializeField] float timeOfDay;
        [SerializeField] float timeOfDawn;
        [SerializeField] float timeOfDusk;

        public UnityEvent Dawn;
        public UnityEvent Dusk;

        private void Start()
        {
            lightTransform = sun.transform;
            Dawn.AddListener(OnDawn);
            Dusk.AddListener(OnDusk);
        }

        private void FixedUpdate()
        {
            lightTransform.Rotate(Vector3.right, 0.1f);
            timeOfDay += Time.fixedDeltaTime;
            if (timeOfDay > lengthOfDay)
            {
                timeOfDay = 0f;
            }else if (Mathf.Approximately(timeOfDay, timeOfDawn))
            {
                Dawn?.Invoke();
            }else if(Mathf.Approximately(timeOfDay, timeOfDusk))
            { 
                Dusk?.Invoke(); 
            }
        }

        public void OnDusk()
        {
            Debug.Log("Dusk");
        }
        public void OnDawn()
        {
            Debug.Log("Dawn");
        }

    }
}