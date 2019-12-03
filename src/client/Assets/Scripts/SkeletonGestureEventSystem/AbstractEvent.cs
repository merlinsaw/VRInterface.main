using System;
using UnityEngine;

namespace SGEventSysthem
{
    public abstract class Event<T> where T : Event<T>
    {

        public string Description;

        public bool HasFired { get; set; }
        public bool Locked { get; set; }
        public delegate void EventListener(T info);
        private static event EventListener listeners;

        public static void RegisterListener(EventListener listener)
        {
            listeners += listener;
        }

        public static void UnregisterListener(EventListener listener)
        {
            listeners -= listener;
        }

        public void FireEvent(bool loop = false)
        {
            if (HasFired == true && loop == false)
            {
                throw new Exception($"This event has already fired, to prevent infinite loops you can't refire an event (Loop is set to {loop})");
            }
            if (!Locked)
            {
                HasFired = true;
                listeners?.Invoke(this as T);
            }
                
        }

        public virtual void ResetEvent() {
            HasFired = false;
            Locked = false;
        }

        public virtual void LockEvent() {
            Locked = true;
        }
    }

    public class DebugEvent : Event<DebugEvent>
    {
        public int VerbosityLevel;
    }

}

