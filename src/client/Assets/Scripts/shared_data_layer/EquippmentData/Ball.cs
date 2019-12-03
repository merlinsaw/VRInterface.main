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

namespace shared_data_layer.EquippmentData
{
    public class Ball
    {

        public string BallId { get; set; }

        public override string ToString()
        {
            StringBuilder xmlAsString = new StringBuilder();
            xmlAsString.AppendLine("BallId = " + BallId);

            return xmlAsString.ToString();
        }
    }
}
