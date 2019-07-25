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

/// <summary>
/// Central class for reporting and listening to game events.
/// </summary>
public class GameEventsController
{

  public delegate void GameEventReported(AbstractGameEvent gameEvent);
  public event GameEventReported OnGameEventReported;

  /// <summary>
  /// Central method to collect all game stats related events.
  /// </summary>
  public void ReportEvent(AbstractGameEvent gameEvent)
  {
	if (OnGameEventReported != null)
	{
	  OnGameEventReported(gameEvent);
	}
  }


}