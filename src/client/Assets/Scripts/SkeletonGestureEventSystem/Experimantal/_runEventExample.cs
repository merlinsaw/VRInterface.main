using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SGEventSysthem
{
    public class _runEventExample : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                SkeletonGesturePush();
            }
        }



        SGPushStartedEvent pushStarted = new SGPushStartedEvent();

        void SkeletonGesturePush()
        {
            if (pushStarted != null && !pushStarted.HasFired)
            {
                pushStarted.Description = "This " + gameObject.name + " has started a Push gesture.";
                pushStarted.Sender = gameObject;
                pushStarted.FireEvent();
                Debug.Log($"<Color=magenta>{pushStarted.Description}</Color>");
            }
        }
    }
}