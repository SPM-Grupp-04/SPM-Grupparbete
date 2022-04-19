using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EgilEventSystem
{
    public class EventSystem : MonoBehaviour
    {
        // Anyone who does anything send things in this.

        private static EventSystem currentVarible;


        private void OnEnable()
        {
            currentVarible = this;
        }

        public static EventSystem current
        {
            get
            {
                if (currentVarible == null)
                {
                    currentVarible = GameObject.FindObjectOfType<EventSystem>();
                }

                return currentVarible;
            }
        }


        private delegate void EventListener(EgilEventSystem.Event e);

        // Should be set
        private Dictionary<System.Type, List<EventListener>> eventListeners;

        public void RegisterListner<T>(System.Action<T> listener) where T : Event
        {
            System.Type eventType = typeof(T);

            if (eventListeners == null)
            {
                eventListeners = new Dictionary<System.Type, List<EventListener>>();
            }

            if (eventListeners.ContainsKey(eventType) == false || eventListeners[eventType] == null)
            {
                eventListeners[eventType] = new List<EventListener>();
            }

            // Wrapp a type converstion around the even listner.
            void Wrapper(Event e)
            {
                listener((T) e);
            }

            eventListeners[eventType].Add(Wrapper);
        }


        public void UnregisterListener<T>(System.Action<T> listener) where T : Event
        {
            System.Type eventType = typeof(T);

            if (eventListeners.ContainsKey(eventType) == false || eventListeners[eventType] == null)
            {
                Debug.Log(" Cannot remove eventType, does not exist in eventListeners");
                return;
            }


            void Wrapper(Event e)
            {
                listener((T) e);
            }

            eventListeners[eventType].Remove(Wrapper);
        }

        // Execute Event.
        public void FireEvent(Event EventInfo)
        {
            System.Type trueEventOInfoClass = EventInfo.GetType();

            if (eventListeners == null || eventListeners[trueEventOInfoClass] == null)
            {
                // No one is listening we are done
                return;
            }

            foreach (EventListener el in eventListeners[trueEventOInfoClass])
            {
                el(EventInfo);
            }
        }
    }
}