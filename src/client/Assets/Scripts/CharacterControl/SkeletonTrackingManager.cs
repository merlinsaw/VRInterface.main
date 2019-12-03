using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SGEventSysthem;



namespace skeleton.Events
{
    public class SkeletonTrackingManager : Skeleton
    {

        private float rightArmLength;
        private float min;
        private float max;
        private float pushDistance;
        public float NormalizedPushDistance { get; private set; }

        private string message = "";

        //public bool Pushing { get; set; }
        public bool DebugMessage;

        //-----------------------------------------------------------------
        //	Skeleton Gesture Push Events
        //-----------------------------------------------------------------

        /// <summary>
        /// Emitted when the Pushing Gesture started.
        /// </summary>
        private SGPushStartedEvent pushStartedEvent = new SGPushStartedEvent();
        /// <summary>
        /// Emitted when the Pushing Gesture was completed.
        /// </summary>
        private SGPushEndedEvent pushEndedEvent = new SGPushEndedEvent();
        /// <summary>
        /// Emitted when the Pushing Gesture is in progess.
        /// </summary>
        private SGPushChangedEvent pushChangedEvent = new SGPushChangedEvent();



        // Start is called before the first frame update
        void Start()
        {
            rightArmLength = GetArmLength("rightArm");
            Debug.Log($"<Color=red> right Arm length: { rightArmLength  } </Color>");

            min = GetArmLength("rightUpperArm");
            max = rightArmLength - rightArmLength * 0.25f;
        }


        // TODO: @msaw - limit the distance to local Y axis.
        /// <summary>
        /// Get the length of a arm segment.
        /// </summary>
        /// <param name="armSegment">"rightArm", "rightUpperArm", "rightForearm", "leftArm", "leftUpperArm", "leftForearm" </param>
        /// <returns></returns>
        private float GetArmLength(string armSegment)
        {
            float armLegth = 0;
            switch (armSegment)
            {
            case "rightArm":
                armLegth = Vector3.Distance(bones.rightUpperArm.position, bones.rightForearm.position)
                + Vector3.Distance(bones.rightForearm.position, bones.rightHand.position);
                return armLegth;
            case "rightUpperArm":
                armLegth = Vector3.Distance(bones.rightUpperArm.position, bones.rightForearm.position);
                return armLegth;
            case "rightForearm":
                Vector3.Distance(bones.rightForearm.position, bones.rightHand.position);
                return armLegth;
            case "leftArm":
                armLegth = Vector3.Distance(bones.leftUpperArm.position, bones.leftForearm.position)
               + Vector3.Distance(bones.leftForearm.position, bones.leftHand.position);
                return armLegth;
            case "leftUpperArm":
                armLegth = Vector3.Distance(bones.leftUpperArm.position, bones.leftForearm.position);
                return armLegth;
            case "leftForearm":
                Vector3.Distance(bones.leftForearm.position, bones.leftHand.position);
                return armLegth;
            default:
                Debug.LogError($"{armSegment} is not defined please check the parameter. Length is {armLegth} ");
                return armLegth;
            }

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            // TODO: @msaw - limit the distance to character facing direction.
            pushDistance = Vector3.Distance(bones.rightUpperArm.position, bones.rightHand.position); // distance between shoulder and hand.


            ////Calculate the normalized float;
            //NormalizedPushDistance = (pushDistance - min) / (max - min);
            ////Clamp the pushDistance float between "min" value and "max" value
            //pushDistance = Mathf.Clamp(pushDistance, min, max);
            ////Clamp the normalized float between 0 and 1
            //NormalizedPushDistance = Mathf.Clamp(NormalizedPushDistance, 0, 1);

            // TODO: @msaw - check what the Remap really outputs
            NormalizedPushDistance = ExtensionMethods.Remap(pushDistance, min, max, 0.00f, 1.00f);

            if (pushStartedEvent != null && pushChangedEvent != null && pushEndedEvent != null)
            {
                if (!pushStartedEvent.HasFired && NormalizedPushDistance <= 0 && bones.rightUpperArm.rotation.z > 0.6f)
                {
                    // pushStartedEvent Args
                    pushStartedEvent.Description = gameObject.name + " started Push on ";
                    pushStartedEvent.Sender = gameObject;

                    pushStartedEvent.FireEvent();

                    pushChangedEvent.ResetEvent();
                    pushEndedEvent.ResetEvent();

                    message = "Started the Push";
                }
                // (check if the start event has a listener - to only limit the action to buttons)
                if (pushStartedEvent.HasFired && NormalizedPushDistance > 0 && NormalizedPushDistance < 1 && bones.rightUpperArm.rotation.z > 0.6f)
                {
                    pushChangedEvent.Description = gameObject.name + $" performing {NormalizedPushDistance * 100:F0}% Push on ";
                    pushChangedEvent.Sender = gameObject;
                    //only perform a push if we are still on the same object, otherwise reset the push start event

                    // did the start event get a listener object? 
                    // did the changed event get a listener object?
                    if (pushStartedEvent.Listener != null && pushChangedEvent.Listener != null)
                    {
                        // get  the changed listener
                        // ...
                        if (pushStartedEvent.Listener.GetHashCode() == pushChangedEvent.Listener.GetHashCode())
                        {
                            // the start listener matches the change listener -> perform the push
                            pushChangedEvent.PushDistance = NormalizedPushDistance;
                            pushChangedEvent.FireEvent(true);
                            Debug.Log("Mark 002");
                        }
                        else
                        {
                            // the start listener is different for the change listener -> reset the change event
                            pushStartedEvent.ResetEvent();
                            pushChangedEvent.ResetEvent();
                            pushChangedEvent.FireEvent();
                            Debug.Log("Mark 003"); // code wont reach here anymore because of the start event control in the Listener
                        }
                    }
                    // did the start event get a listener object? -> run the change event once to get a listener
                    else if (pushStartedEvent.Listener != null && pushChangedEvent.Listener == null)
                    {
                        pushChangedEvent.ResetEvent();
                        pushChangedEvent.FireEvent();
                        Debug.Log("Mark 001");
                    }
                    // the start event has no lister -> reset the start event
                    else
                    {
                        pushStartedEvent.ResetEvent();
                        Debug.Log("Mark 000");
                    }

                    message = $"Pushing {(NormalizedPushDistance * 100):F0}%\n";
                    message += $"rotationX {(bones.rightUpperArm.rotation.x):F3}\n";
                    message += $"rotationZ {(bones.rightUpperArm.rotation.z):F3}\n";
                }
                if (pushStartedEvent.HasFired && pushChangedEvent.HasFired && NormalizedPushDistance >= 1 && bones.rightUpperArm.rotation.z > 0.6f)
                {

                    

                    if (!pushEndedEvent.HasFired)
                    {
                        // pushEndedEvent Args
                        pushEndedEvent.Description = gameObject.name + " ended Push on ";
                        pushEndedEvent.Sender = gameObject;

                        pushEndedEvent.FireEvent();

                        pushChangedEvent.ResetEvent();
                        pushStartedEvent.ResetEvent();
                    }

                    message = $"rotationX {(bones.rightUpperArm.rotation.x):F3}\n";
                    message += $"rotationZ {(bones.rightUpperArm.rotation.z):F3}\n";
                }
                else
                { message = ""; }
            }
            else
            {
                pushStartedEvent = new SGPushStartedEvent();
                pushChangedEvent = new SGPushChangedEvent();
                pushEndedEvent = new SGPushEndedEvent();
            }

        }

        //private GUIStyle guiStyle = new GUIStyle();
        //void OnGUI()
        //{
        //    if (DebugMessage)
        //        guiStyle.fontSize = 50;
        //    GUI.Label(new Rect(Screen.width / 2, Screen.height / 4, 500f, 500f), message, guiStyle);
        //}
    }
}

