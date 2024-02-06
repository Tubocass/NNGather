using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace gather
{
    public class GlobalEventManager : MonoBehaviour
    {
        Dictionary<string, UnityEvent> eventDictionary = new Dictionary<string, UnityEvent>();
        private static GlobalEventManager eventManager;

        public static GlobalEventManager instance
        {
            get
            {
                if (!eventManager)
                {
                    eventManager = FindFirstObjectByType(typeof(GlobalEventManager)) as GlobalEventManager;

                    if (!eventManager)
                    {
                        Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                    } else
                    {
                        eventManager.Init();
                    }
                }

                return eventManager;
            }
        }

        void Init()
        {
            if (eventDictionary == null)
            {
                eventDictionary = new Dictionary<string, UnityEvent>();
            }
        }

        public static void StartListening(string eventName, UnityAction listener)
        {
            if (instance.eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
            {
                thisEvent.AddListener(listener);
            } else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                instance.eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, UnityAction listener)
        {
            if (eventManager == null) 
                return;

            if (instance.eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void TriggerEvent(string eventName)
        {
            if (instance.eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
            {
                thisEvent.Invoke();
            }
        }
    }
}