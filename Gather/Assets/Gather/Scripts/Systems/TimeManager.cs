using UnityEngine;
using UnityEngine.Events;

namespace gather
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] Light sun;
        Transform lightTransform;
        [SerializeField] int lengthOfDay;
        [SerializeField] int timeOfDay;
        [SerializeField] int timeOfDawn;
        [SerializeField] int timeOfDusk;

        public UnityEvent OnDawn;
        public UnityEvent OnDusk;

        private void Start()
        {
            lightTransform = sun.transform;
            OnDawn.AddListener(AtDawn);
            OnDusk.AddListener(AtDusk);
        }

        private void FixedUpdate()
        {
            lightTransform.Rotate(Vector3.right, 0.1f);
            timeOfDay++;
            if (timeOfDay > lengthOfDay)
            {
                timeOfDay = 0;
            } 
            if (timeOfDay == timeOfDawn)
            {
                OnDawn?.Invoke();
            }else if(timeOfDay == timeOfDusk)
            { 
                OnDusk?.Invoke(); 
            }
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