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
using VRTK;

#endregion

namespace Assets.Buttons
{
    public class VRTK_MSAW_InteractTouch : VRTK_InteractTouch
    {

        protected override void CheckStopTouching()
        {
            if (touchedObject != null)
            {
                VRTK_InteractableObject touchedObjectScript = touchedObject.GetComponent<VRTK_InteractableObject>();

                //If it's being grabbed by the current touching object then it hasn't stopped being touched.
                //if (touchedObjectScript != null && touchedObjectScript.GetGrabbingObject() != gameObject)
                //{
                //    StopTouching(touchedObject);
                //}
                if (touchedObjectScript != null && touchedObjectScript.GetUsingObject() != gameObject)
                {
                    StopTouching(touchedObject);
                }
            }
        }

    }
}
