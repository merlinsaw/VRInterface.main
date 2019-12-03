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


#endregion

using shared_data_layer.Lobby;
using shared_data_layer.SinglePlayer;


namespace shared_data_layer
{
    public class UserDTO : AbstractPlayerDTO, IEnemy
    {
    
        public UserSinglePlayerProgressDTO UserSinglePlayerProgress { get; set; }

        public UserLobby UserLobby { get; set; }
      
    }
}
