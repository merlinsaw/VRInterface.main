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
    public class UserRacket {

        public string RacketId { get; set; }

        public UserRacket() { }

        public override string ToString()
        {
            return ("UserRacket(Id: " + RacketId + ")");
        }

        public UserRacket Clone()
        {
            return new UserRacket()
            {
                RacketId = this.RacketId,
            };
        }

    }
}
