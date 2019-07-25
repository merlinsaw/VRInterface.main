//-----------------------------------------------------------------
//	this will trigger the grab state automatically after some time
//-----------------------------------------------------------------

#region using


using System.Collections;
using UnityEngine;
using VRTK;


#endregion

namespace Assets.Buttons
{
    public class InteractableButton : MonoBehaviour
    {
        public VRTK_InteractableObject linkedObject;
        public float delayTillUsable;
        public VRTK_ObjectTouchAutoInteract autoGrab;
        protected bool autoUse;
        


        protected virtual void OnEnable()
        {
            linkedObject = (linkedObject == null ? GetComponent<VRTK_InteractableObject>() : linkedObject);

            if (linkedObject != null)
            {
                linkedObject.InteractableObjectTouched += InteractableObjectTouched;
                linkedObject.InteractableObjectUntouched += InteractableObjectUntouched;

                //linkedObject.InteractableObjectUsed += InteractableObjectUsed2;
                //linkedObject.InteractableObjectUnused += InteractableObjectUnused;
            }

            //autoGrab = linkedObject.GetComponent<VRTK_ObjectTouchAutoInteract>();
          
            
        }

        protected virtual void OnDisable()
        {
            if (linkedObject != null)
            {
                linkedObject.InteractableObjectTouched -= InteractableObjectTouched;
                linkedObject.InteractableObjectUntouched -= InteractableObjectUntouched;

                //linkedObject.InteractableObjectUsed -= InteractableObjectUsed2;
                //linkedObject.InteractableObjectUnused -= InteractableObjectUnused;

            }

           
        }

        protected virtual void InteractableObjectTouched(object sender, InteractableObjectEventArgs e)
        {

            // run some timer
            StartCoroutine(DelayedGrabAction(delayTillUsable));
            //autoGrab.grabOnTouchWhen = VRTK_ObjectTouchAutoInteract.AutoInteractions.NoButtonHeld;
            // grab the object
            
            autoUse = true;
            Debug.Log($"<Color=green> {sender}, {e} </Color>");
        }

        protected virtual void InteractableObjectUntouched(object sender, InteractableObjectEventArgs e)
        {
            Debug.Log($"<Color=blue> {sender}, {e} </Color>");
            autoUse = false;
            //autoGrab.grabOnTouchWhen = VRTK_ObjectTouchAutoInteract.AutoInteractions.Never;

            InteractableObjectUnused2(sender, e);
            this.StopAllCoroutines();
            linkedObject.InteractableObjectUsed -= InteractableObjectUsed2;
        }

        protected virtual void InteractableObjectUsed2(object sender, InteractableObjectEventArgs e)
        {
            Debug.Log("<Color=red> ++++Using++++ </Color>" );
        }

        protected virtual void InteractableObjectUnused2(object sender, InteractableObjectEventArgs e)
        {
            Debug.Log("<Color=blue> ----Unusing---- </Color>");
        }

        protected virtual IEnumerator DelayedGrabAction(float deleay)
        {
            float
                fadeTime = deleay,
                elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }
            // grab the object
            linkedObject.InteractableObjectUsed += InteractableObjectUsed2;
            //autoGrab.useOnTouchWhen = VRTK_ObjectTouchAutoInteract.AutoInteractions.NoButtonHeld;
            Debug.Log("DeleaydGrabAction");
        }
    }
}
