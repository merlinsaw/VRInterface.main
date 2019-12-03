//-------------------------------------------------------
//	Skeleton Gesture Push Changed Event Data
//-------------------------------------------------------

#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Valve.VR;

#endregion

namespace SGEventSysthem
{
    public class SGPushChangedEvent : Event<SGPushChangedEvent>
    {

        // Game object
        public GameObject Sender { get; set; }
        public GameObject Listener { get; set; }
        public float PushDistance { get; set; }

        //// constructor
        //public SGPushStartedEvent(GameObject sender, string description = "") {
        //    Description = description;
        //    GO = sender;

        //    //FireEvent();
        //}





        public override void ResetEvent()
        {
            PushDistance = 0f;
            Listener = null;
            base.ResetEvent();

        }



        


    }
}
