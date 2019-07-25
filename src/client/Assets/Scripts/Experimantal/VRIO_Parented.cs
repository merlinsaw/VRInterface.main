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
    public class VRIO_Parented : VRInteractableObject
    {

        protected Rigidbody rigidBody;
        protected bool originalKinematicState;
        protected Transform originalParent;

        public EVRButtonId pickupButton = EVRButtonId.k_EButton_SteamVR_Trigger;

        //-------------------------------------------------------
        //
        //-------------------------------------------------------
        void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();

            // Capture pbject's original parent and kinematic state
            originalParent = transform.parent;
            originalKinematicState = rigidBody.isKinematic;
        }
        //-------------------------------------------------------
        //
        //-------------------------------------------------------
        public override void ButtonPressDown(EVRButtonId button, VRControllerInput controller)
        {
            //If pickup button was pressed
            if (button == pickupButton)
                Pickup(controller);
        }
        //-------------------------------------------------------
        //
        //-------------------------------------------------------
        public override void ButtonPressUp(EVRButtonId button, VRControllerInput controller)
        {
            //If pickup button was released
            if (button == pickupButton)
                Release(controller);
        }
        //-------------------------------------------------------
        //
        //-------------------------------------------------------
        public void Pickup(VRControllerInput controller)
        {
            rigidBody.isKinematic = true;
            transform.SetParent(controller.gameObject.transform);
        }

        //-------------------------------------------------------
        //
        //-------------------------------------------------------
        public void Release(VRControllerInput controller)
        {
            // Make sure that the hand triggering the Release method is still the hand holding the object
            if (transform.parent == controller.gameObject.transform)
            {
                //Return previous kinematic state
                rigidBody.isKinematic = originalKinematicState;

                //Set object's parent to its original parent
                if (originalParent != controller.gameObject.transform)
                {
                    //Ensure original parent recorded wasn't somehow the controller (failsafe)
                    transform.SetParent(originalParent);
                }
                else
                {
                    transform.SetParent(null);
                }
#if missing_old_SteamVR_Controller
                //Trow object
                rigidBody.velocity = controller.device.velocity;
                rigidBody.angularVelocity = controller.device.angualarVelocity;
#endif
            }
        }
    }
}
