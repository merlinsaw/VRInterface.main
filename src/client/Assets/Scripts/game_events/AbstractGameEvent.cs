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
//using Valve.VR;

#endregion



public enum GameEventType
{
  GameDataReceived,
  PlayerDataReceived,
  MainMenuActivated,
  SinglePlayerLevelEntered,
  SinglePlayerLevelLeft


}

/// <summary>
/// Base class for all game events.
/// </summary>
public abstract class AbstractGameEvent
{

  public abstract GameEventType GameEventType { get; }
}


