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
    public class UserBall
    {

        public string BallId { get; set; }

        public UserBall() { }

        public override string ToString()
        {
            return ("UserBall(Id: " + BallId + ")");
        }

        public UserBall Clone()
        {
            return new UserBall()
            {
                BallId = this.BallId,
            };
        }

    }
}
