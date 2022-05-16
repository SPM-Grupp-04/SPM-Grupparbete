using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EgilEventSystem
{
    public class EventSystem : MonoBehaviour
    {
        // Anyone who does anything send things in this.

        private static EventSystem currentVariable;


        private void OnEnable()
        {
            currentVariable = this;
        }

        public static EventSystem current
        {
            get
            {
                if (currentVariable == null)
                {
                    currentVariable = GameObject.FindObjectOfType<EventSystem>();
                }

                return currentVariable;
            }
        }


        private delegate void EventListener(EgilEventSystem.Event e);

        // Should be set
        private Dictionary<System.Type, List<EventListener>> eventListeners;

        public void RegisterListener<T>(System.Action<T> listener) where T : Event
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

            // Wrap a type conversion around the event listener.
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