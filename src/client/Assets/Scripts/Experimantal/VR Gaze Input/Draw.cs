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

    public class Draw : MonoBehaviour
    {
        public GameObject lines;
        public void showLines()
        {
            lines.SetActive(true);
        }
        public void hideLines()
        {
            lines.SetActive(false);
        }
    }


}

