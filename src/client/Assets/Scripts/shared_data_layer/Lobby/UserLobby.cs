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

namespace shared_data_layer.Lobby
{
    public class UserLobby : MonoBehaviour
    {

        /// <summary>
        /// The Level id of the level the user is currently playing
        /// </summary>
        public string CurrentLevelId { get; set; }

    }
}
