//-------------------------------------------------------
//	
//-------------------------------------------------------

#region using

using shared_data_layer;

#endregion

/// <summary>
/// Concrete implementation of a game event.
/// </summary>
public class EventPlayerDataReceived : AbstractGameEvent
{

  public override GameEventType GameEventType
  {
	get { return GameEventType.PlayerDataReceived; }
  }

  public UserDTO UserDTO { get; set; }
}