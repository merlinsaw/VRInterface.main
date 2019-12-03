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
/// Concrete implementation of a game event.
/// </summary>
public class EventMainMenuButtonActivated : AbstractGameEvent
{

  public override GameEventType GameEventType
  {
	get { return GameEventType.MainMenuActivated; }
  }

  public string ButtonName { get; set; }

  public EventMainMenuButtonActivated(string buttonName)
  {
	ButtonName = buttonName;
  }

}




