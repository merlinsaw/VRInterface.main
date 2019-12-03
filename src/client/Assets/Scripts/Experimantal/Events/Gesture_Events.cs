//-------------------------------------------------------
//	
//-------------------------------------------------------

#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

#endregion

namespace Assets.EventTemplate

{
    public class Gesture_UnityEvents : Gesture_UnityEvents<Gesture_UnityEvents>
    {


        [Serializable]
        public sealed class GestureEvent : UnityEvent<object, GestureEventArgs> { }

        protected override void AddListeners(Gesture_UnityEvents component)
        {

        }

        protected override void RemoveListeners(Gesture_UnityEvents component)
        {

        }
    }

    public class GestureEventArgs
    {

    }
}
