using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SGEventSysthem
{
    public class SkeletonGestureListener : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            SGPushStartedEvent.RegisterListener(OnSGPushStarted);
            SGPushChangedEvent.RegisterListener(OnSGPushChanged);
            SGPushEndedEvent.RegisterListener(OnSGPushEnded);
            SGPushCanceledEvent.RegisterListener(OnSGPushCanceled);
        }

        void OnDestroy()
        {
            SGPushStartedEvent.UnregisterListener(OnSGPushStarted);
            SGPushChangedEvent.UnregisterListener(OnSGPushChanged);
            SGPushEndedEvent.UnregisterListener(OnSGPushEnded);
            SGPushCanceledEvent.UnregisterListener(OnSGPushCanceled);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnSGPushStarted(SGPushStartedEvent gesture)
        {
            Debug.Log($"<Color=magenta>{gesture.Description}</Color>");
        }

        void OnSGPushChanged(SGPushChangedEvent gesture)
        {
            Debug.Log($"<Color=yellow>{gesture.Description}</Color>");
        }

        void OnSGPushEnded(SGPushEndedEvent gesture)
        {
            Debug.Log($"<Color=green>{gesture.Description}</Color>");
        }

        void OnSGPushCanceled(SGPushCanceledEvent gesture)
        {
            Debug.Log($"<Color=red>{gesture.Description}</Color>");
        }
    }
}