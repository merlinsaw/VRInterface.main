//-------------------------------------------------------
//	Basic events for buttons.
//-------------------------------------------------------

#region using

using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#endregion

namespace AdvancedUI {

  [Serializable]
  public class BaseEventDataEvent : UnityEvent<BaseEventData> { }
  [Serializable]
  public class PointerEventDataEvent : UnityEvent<PointerEventData> { }
  [Serializable]
  public class ColliderEventDataEvent : UnityEvent<ColliderEventData> { }

  [Serializable]
  public class InteractableEvents {
	public BaseEventDataEvent
		onSubmit = new BaseEventDataEvent(),
		onSelect = new BaseEventDataEvent(),
		onDeselect = new BaseEventDataEvent();
	public PointerEventDataEvent
		onPointerClick = new PointerEventDataEvent(),
		onPointerDown = new PointerEventDataEvent(),
		onPointerUp = new PointerEventDataEvent(),
		onPointerEnter = new PointerEventDataEvent(),
		onPointerExit = new PointerEventDataEvent();
#if using_collision_events
		public ColliderEventDataEvent
			onTriggerEnters = new ColliderEventDataEvent(),
			onTriggerExits = new ColliderEventDataEvent(),
			onSubmitDelayed = new ColliderEventDataEvent();
#endif
	}
}