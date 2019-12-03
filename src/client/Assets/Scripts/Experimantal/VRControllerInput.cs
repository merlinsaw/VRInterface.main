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

    public class VRControllerInput : MonoBehaviour
    {


        // Should only ever be one, but just in case one gets stuck
        protected Dictionary<EVRButtonId, List<VRInteractableObject>> pressDownObjects;


        // TODO: @msaw: check  SteamVR_TrackedObject !
        // TODO: @msaw: check  SteamVR_Controller.Device !
        // Controller References
        protected SteamVR_TrackedObject trackedObj;
#if missing_old_SteamVR_Controller
        public SteamVR_Controller.Device device
        {
            get
            {
                return SteamVR_Controller.Input((int)trackedObj.index);
            }
        }
#endif

        public delegate void TouchpadPress();
        public static event TouchpadPress OnTouchpadPress;

        // 
        protected List<Enum> buttonsTracked;
        //-------------------------------------------------------
        //	
        //-------------------------------------------------------
        void Awake()
        {
            trackedObj = GetComponent<SteamVR_TrackedObject>();

            pressDownObjects = new Dictionary<EVRButtonId, List<VRInteractableObject>>();

            /*
             * using an enum as a key for Dictionaries 
             * can lead to some memory issues in Mono, as discussed here.
             * https://stackoverflow.com/questions/26280788/dictionary-enum-key-performance
             * I’m going to leave it as an enum for simplicity’s sake,
             * but for optimizations, this may be worth reading into.
             */

            // TODO: @msaw: enum boxing performance optimisations
            //List the buttons you send inputs to the controller for
            buttonsTracked = new List<Enum>()
            {
                EVRButtonId.k_EButton_SteamVR_Trigger,
                EVRButtonId.k_EButton_Grip
            };
        }

        //-------------------------------------------------------
        //	when collider hits...
        //-------------------------------------------------------
        void OnTriggerStay(Collider collider)
        {
            // if collider has a rigid body to report to 
            // all collider downwards report to the colider upwards in the hirarchy
            if (collider.attachedRigidbody != null)
            {


                // TODO: @msaw: try that without a try catch sentence
                try
                {
                    VRInteractableObject[] interactables = collider.GetComponentsInChildren<VRInteractableObject>(false);
                    VRInteractableObject interactable = null;
                    //If rigidbodiy's object has an interactable item <VRInteractableObject> script, iterate through them

                    for (int i = 0; i < interactables.Length; i++)
                    {
                        interactable = interactables[i];

                        //Check through all desried buttons to see if any are pressed
                        for (int b = 0; b < buttonsTracked.Count; b++)
                        {
                            // if tracked button is pressed
                            EVRButtonId button = (EVRButtonId)buttonsTracked[b];
                            // TODO: @msaw: if ceartain amount of time has passed event is true
#if missing_old_SteamVR_Controller
                            if (device.GetPressDown(button))
                            {

                                //If we haven't already sent the button press message to this interactable
                                //Safeguard against objects that have multiple colliders for one interactable script
                                if (!pressDownObjects.ContainsKey(button) || !pressDownObjects[button].Contains(interactable))
                                {
                                    //Send button press through to interactable script
                                    interactable.ButtonPressDown(button, this);

                                    //Add interactable script to a dictionary flagging it to recieve notice
                                    //when that same button is released
                                    if (!pressDownObjects.ContainsKey(button))
                                        pressDownObjects.Add(button, new List<VRInteractableObject>());

                                    pressDownObjects[button].Add(interactable);
                                    
                               
                                }
                            }
#endif
                        }
                    }
                }
                catch (Exception)
                {

                }
            }


        }

        //-------------------------------------------------------
        //	
        //-------------------------------------------------------
        void Update()
        {

            //Check through all desired buttons to see if any have been released
            EVRButtonId[] pressKeys = pressDownObjects.Keys.ToArray();
            for (int i = 0; i < pressKeys.Length; i++)
            {
                //If tracked button is released
                // TODO: @msaw: here we need a bone gesture to confirm the release of the object
#if missing_old_SteamVR_Controller
                if (device.GetPressUp(pressKeys[i]))
                {
                    //Get all tracked objects in that button's "pressed" list
                    List<VRInteractableObject> releaseObjects = pressDownObjects[pressKeys[i]];
                    for (int j = 0; j < releaseObjects.Count; j++)
                    {
                        //Send button release through to interactable script
                        releaseObjects[j].ButtonPressUp(pressKeys[i], this);
                    }

                    //Clear 
                    pressDownObjects[pressKeys[i]].Clear();
                }
#endif
            }
        }
    }


}
