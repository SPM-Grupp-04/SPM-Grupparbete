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

        public delegate void EventListener(EgilEventSystem.Event e);

        // Should be set
        private Dictionary<System.Type, List<EventListener>> eventListeners;

        public EventListener RegisterListener<T>(System.Action<T> listener) where T : Event
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
                listener((T)e);
            }

            eventListeners[eventType].Add(Wrapper);
            return Wrapper;
        }

        public void UnregisterListener<T>(EventListener listener) where T : Event
        {
            System.Type eventType = typeof(T);

            if (eventListeners.ContainsKey(eventType) == false || eventListeners[eventType] == null)
            {
                Debug.Log(" Cannot remove eventType, does not exist in eventListeners");
                return;
            }

            eventListeners[eventType].Remove(listener);
        }

        // Execute Event.
        public void FireEvent(Event firedEvent)
        {
            System.Type firedEventType = firedEvent.GetType();

            if (eventListeners == null || eventListeners[firedEventType] == null)
            {
                // No one is listening we are done
                return;
            }

            //regular for-loop instead of foreach; if an event listener is unregistered mid-loop it won't throw an error
            for (int i = 0; i < eventListeners[firedEventType].Count; i++)
            {
                eventListeners[firedEventType][i]?.Invoke(firedEvent);
            }
        }

        //not really needed, but the event system is throwing a lot of exceptions on application quit and this is due diligence
        private void OnDestroy()
        {
            foreach (var v in eventListeners)
            {
                eventListeners[v.Key].Clear();
            }
        }
    }
}