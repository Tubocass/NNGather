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
            timeOfDay += Time.fixedDeltaTime;
            if (timeOfDay > lengthOfDay)
            {
                timeOfDay = 0f;
            }else if (Mathf.Approximately(timeOfDay, timeOfDawn))
            {
                OnDawn?.Invoke();
            }else if(Mathf.Approximately(timeOfDay, timeOfDusk))
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