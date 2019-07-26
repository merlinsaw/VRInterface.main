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

using shared_data_layer;
using shared_data_layer.Lobby;

/// <summary>
/// Method to contain all user information relevant to the client. For example equipped weapons and distractions.
/// </summary>
public sealed class User
{

    public User()
    {
        UserDTO = new UserDTO();
    }

    public string Name
    {
        get
        {
            if (UserDTO != null && string.IsNullOrEmpty(UserDTO.Name) == false)
            {
                return (UserDTO.Name);
            }
            else
            {
                //return (EncryptedPlayerPrefs.GetString(PlayerPrefsKeys.userName));
                return "Max Mustermann";
            }
        }
    }

    /// <summary>
    /// Getter that returns the proper SP name of the user. This property is necessary because the username is different in case of the first 3 game-levels.
    /// </summary>
    public string SinglePlayerName
    {
        get
        {
            string spName = "";
            switch (UserLobby.CurrentLevelId)
            {
            case "L001":
            case "L002":
            case "L003":
                spName = Loca.Get("Level.UserName." + UserLobby.CurrentLevelId);
                break;
            default:
                spName = Name;
                break;
            }
            return (spName);
        }
    }

    public UserDTO UserDTO { get; set; }
    public UserEquipmentDTO UserEquipment { get { return (UserDTO.UserEquipment); } set { UserDTO.UserEquipment = value; } }

    

//#if UNITY_EDITOR || UNITY_ENGINE
//    [JsonConverter(typeof(UtcDateTimeConverter))]
//#endif
    

    /// <summary>
    /// Retrieves the current player level of the user.
    /// </summary>
    //public PlayerLevel GetPlayerLevel(BalancingData balancingData)
    //{
    //    PlayerLevel playerLevel = balancingData.PlayerProgressData.FindHighestUnlockedLevel(numPoints);
    //    return (playerLevel);
    //}

    //public PlayerLevel GetPlayerLevelById(BalancingData balancingData, int levelId)
    //{
    //    PlayerLevel playerLevel = balancingData.PlayerProgressData.FindLevelById(levelId);
    //    return (playerLevel);
    //}


    public int NrTrophies
    {
        get
        {
            return UserDTO.NrTrophies;
        }
    }


    public UserLobby UserLobby
    {
        get
        {
            return UserDTO.UserLobby;
        }
    }
}