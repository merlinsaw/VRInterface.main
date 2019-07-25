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
using Valve.VR;

#endregion

namespace Assets.Playground
{
    public class VRInteractableObject : MonoBehaviour
    {

        /// <summary>
        /// Called when button is pressed down while controller is inside object
        /// </summary>
        /// <param name="button"></param>
        /// <param name="controller"></param>
        public virtual void ButtonPressDown(EVRButtonId button, VRControllerInput controller) { }

        /// <summary>
        /// Called when button is released after an object has been "grabbed".
        /// </summary>
        /// <param name="button"></param>
        /// <param name="controller"></param>
        public virtual void ButtonPressUp(EVRButtonId button, VRControllerInput controller) { }


    }
}
