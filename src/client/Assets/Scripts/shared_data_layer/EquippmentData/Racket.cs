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
    public class Racket
    {

        public string RacketId { get; set; }

        public override string ToString()
        {
            StringBuilder xmlAsString = new StringBuilder();
            xmlAsString.AppendLine("RacketId = " + RacketId);

            return xmlAsString.ToString();
        }

    }
}
