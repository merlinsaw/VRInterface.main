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


    public class IconManager : MonoBehaviour
    {
        private static readonly _Logger log = new _Logger(typeof(IconManager));

    #region Miscellaneous

    public Sprite iconLogo;
    public Sprite iconTennisBall;

    public Sprite IconLogo
    {
        get
        {
            return IconLogo;
        }
    }

    public Sprite IconTennisBall
    {
        get
        {
            return IconTennisBall;
        }
    }

    #endregion

}

