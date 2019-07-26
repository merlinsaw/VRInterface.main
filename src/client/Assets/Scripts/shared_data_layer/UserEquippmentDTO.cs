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

using shared_data_layer.EquippmentData;

namespace shared_data_layer { 
public class UserEquipmentDTO
{

    public UserRacket ActiveRacket { get; set; }
    private List<UserRacket> rackets = new List<UserRacket>();
    public List<UserRacket> Rackets
    {
        get { return rackets; }
        set { rackets = value; }
    }

    public UserBall ActiveBall { get; set; }
    private List<UserBall> balls = new List<UserBall>();
    public List<UserBall> Balls
    {
        get { return balls; }
        set { balls = value; }
    }

    public UserRacket GetUserRacket(string racketId)
    {
        return (Rackets.Find(item => item.RacketId == racketId));
    }

    public UserBall GetUserBall(string ballId)
    {
        return (Balls.Find(item => item.BallId == ballId));
    }

    

    public override string ToString()
    {
        StringBuilder equipmentAsString = new StringBuilder();
        equipmentAsString.AppendLine("Rackets: " + Rackets.Count);
        foreach (UserRacket userRacket in Rackets)
        {
            equipmentAsString.AppendLine("* Id: " + userRacket.RacketId);
        }
        equipmentAsString.AppendLine("Balls: " + Balls.Count);
        foreach (UserBall userBall in Balls)
        {
            equipmentAsString.AppendLine("* Id: " + userBall.BallId);
        }

        return (equipmentAsString.ToString());
    }

    /// <summary>
    /// Updates/replaces a specific UserRacket
    /// </summary>
    /// <param name="userRacket"></param>
    public void UpdateRacket(UserRacket userRacket)
    {
        Rackets.RemoveAll(racket => racket.RacketId == userRacket.RacketId);
        Rackets.Add(userRacket);

        // check if this is the active racket and if so, replace
        if (ActiveRacket.RacketId == userRacket.RacketId)
        {
            ActiveRacket = userRacket;
        }
    }
}
}
