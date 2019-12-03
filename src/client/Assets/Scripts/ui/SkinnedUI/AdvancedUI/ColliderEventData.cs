//-------------------------------------------------------
//	Event data for a simple collision event
//-------------------------------------------------------

#region using

using UnityEngine;

#endregion

namespace AdvancedUI
{
  public class ColliderEventData
  {

	public GameObject Controller { get; set; }
	public GameObject Collider { get; set; }
	

	public ColliderEventData(GameObject controller, GameObject collider)
	{
	  Controller = controller;
	  Collider = collider;
	}
	


  }
}
