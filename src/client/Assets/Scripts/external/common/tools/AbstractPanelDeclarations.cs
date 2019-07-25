using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;


/// <summary>
/// Common base-class for the helper classes that allow to declare gui elements for easier use from the code.
/// </summary>
public abstract class AbstractPanelDeclarations : MonoBehaviour //: NestedControllerBehaviour
{

  protected new readonly _Logger log = new _Logger(typeof(AbstractPanelDeclarations));


  //protected UIPanel GUIPanel { get; private set; }
  //protected static FSMSystem FsmSystem { get; private set; }
  private bool AutoGUISetActive { get; set; }
  //public AbstractFSMState State { get; protected set; } //Holds either the state that is currently active where the panel is registered in, or the last one registered.
  //protected List<AbstractFSMState> States { get; set; } //Holds all states the panel is registered in.
 // protected float TimeInState
 // {
	//get
	//{
	//  if (State != null)
	//  {
	//	return (State.TimeInState);
	//  }
	//  return (0);
	//}
 // }

  /// <summary>
  /// Members for forwarding GUI button events.
  /// </summary>
  public delegate void ButtonClickedHandler(GameObject sender);
  public event ButtonClickedHandler OnButtonClicked;



  public void OnButton(MonoBehaviour sender)
  { //To be invoked by ActiveAnimation.cs
	if (sender == null)
	{
	  log.Error(_Logger.User.Msaw, "(MonoBehaviour)sender == null. Not forwarding button event.");
	  return;
	}
	OnButton(sender.gameObject);
  }

  public void OnButton(GameObject sender)
  {
	if (sender == null)
	{
	  log.Error(_Logger.User.Msaw, "(GameObject)sender == null. Not forwarding button event.");
	  return;
	}

	log.DebugMS("AbstractPanelDeclarations.OnButton( " + sender.name + " )");

	if (OnButtonClicked != null)
	{
	  OnButtonClicked(sender);
	}

	OnGUIButton(sender);
  }

 // public void PanelInitialize(FSMSystem fsmSystem, bool autoGUISetActive, AbstractFSMState state)
 // {
	//AutoGUISetActive = autoGUISetActive;
	//if (FsmSystem != null)
	//{
	//  if (FsmSystem != fsmSystem)
	//  {
	//	log.Error(Logger.User.Msaw, "Trying to register panel in more than one states that are in different FsmSystems. This is currently not tested and shouldn't be done.");
	//  }
	//}
	//FsmSystem = fsmSystem;
	//if (States == null)
	//{
	//  States = new List<AbstractFSMState>();
	//}
	//States.Add(state);
	//State = state;
	//GUIPanel = this.GetComponent<UIPanel>();
	//if (GUIPanel != null && AutoGUISetActive == true)
	//{
	//  SetPanelActive(false);
	//}
	//OnInitialize();
 // }

 // public void PanelEnter(object onEnterParams)
 // {
	//State = States.Find(item => item.StateName == FsmSystem.CurrentStateName); //Fetch the state the panel is registered in that is currently active.
	//if (GUIPanel != null && AutoGUISetActive == true)
	//{
	//  SetPanelActive(true);
	//}
	//OnEnter(onEnterParams);
 // }

  public void PanelUpdate()
  {
	OnUpdate();
  }

  public void PanelLateUpdate()
  {
	OnLateUpdate();
  }

 // public void PanelLeave(bool deactivatePanel)
 // {
	//if (GUIPanel != null && AutoGUISetActive == true)
	//{
	//  SetPanelActive(false);
	//}
	//OnLeave();
 // }

  public void PanelOnGUI()
  {
	OnGUICustom();
  }

  //Overridable methods.
  protected virtual void OnInitialize() { }
  protected virtual void OnAwake() { }
  protected virtual void OnEnter(object onEnterParams) { }
  protected virtual void OnLeave() { }
  protected virtual void OnUpdate() { }
  protected virtual void OnLateUpdate() { }
  protected virtual void OnGUICustom() { }
  protected virtual void OnGUIButton(GameObject sender) { }

  //Helper method.
  public void ChangeState(string newStateName, object onEnterParam = null)
  {
	//FsmSystem.ChangeState(newStateName, onEnterParam);
  }

  public void SetPanelActive(bool flag)
  {
#if UNITY_3_5
	  GUIPanel.gameObject.SetActiveRecursively(flag);
#else
	//GUITools.SetActive(GUIPanel.gameObject, flag);
	// //LegacyTools.SetActiveRecursively(GUIPanel.gameObject, flag);
#endif
  }

  public virtual void ShowPanel()
  {
	SetPanelActive(true);
  }

  public virtual void HidePanel()
  {
	SetPanelActive(false);
  }
}