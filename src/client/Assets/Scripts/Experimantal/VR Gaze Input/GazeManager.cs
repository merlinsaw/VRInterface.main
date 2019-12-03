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

namespace Assets.VR_Gaze_Input
{
    public class GazeManager : MonoBehaviour
    {
        public float sightlength = 100.0f;
        public GameObject selectedObj;
        void FixedUpdate()
        {
            RaycastHit seen;
            Ray raydirection = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(raydirection, out seen, sightlength))
            {
                if (seen.collider.tag == "Ground")
                {
                    if (selectedObj != null)
                    {
                        if (seen.transform.gameObject.GetComponent<Draw>())
                        {
                            if (selectedObj.GetComponent<Draw>() != seen.transform.gameObject.GetComponent<Draw>())
                            {
                                selectedObj.GetComponent<Draw>().hideLines();
                            }
                        }
                    }
                    selectedObj = seen.transform.gameObject;
                    if (selectedObj)
                    {
                        selectedObj.GetComponent<Draw>().showLines();
                    }
                }
                else if (seen.collider.tag == "UI")
                {
                    Debug.Log("Hello");
                }
            }
        }
    }
}
