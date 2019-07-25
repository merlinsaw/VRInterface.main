using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Abstract baseclass that allows for states to have separate MonoBehaviour controller classes associated with them.
/// This in turn for example allows to specify relations through the unity inspector and use them in code.
/// </summary>
public abstract class AbstractFSMStateController : NestedControllerBehaviour
{

    protected new static readonly _Logger log = new _Logger(typeof(AbstractFSMStateController));

    public GameObject GUIPanel = null; //In case GUI is used, and this variable is assigned, the according panel will be enabled/disabled automatically when the state is entered or left.

    protected FSMSystem FsmSystem { get; private set; }
    protected AbstractFSMState State { get; private set; }
    protected AbstractPanelDeclarations PrimaryPanelDeclarations { get; private set; } //Controllers can only handle a single panel declaration. However, states may handle multiple panel declarations at once.
    private bool AutoGUISetActive { get; set; }

    public void ControllerInitialize(FSMSystem fsmSystem, AbstractFSMState state, AbstractPanelDeclarations primaryPanelDeclarations, bool autoGUISetActive)
    {
        AutoGUISetActive = autoGUISetActive;
        FsmSystem = fsmSystem;
        State = state;
        PrimaryPanelDeclarations = primaryPanelDeclarations;
        if (GUIPanel != null && AutoGUISetActive == true)
        {
            SetPanelActive(false);
        }
        if (PrimaryPanelDeclarations != null)
        {
            PrimaryPanelDeclarations.OnButtonClicked += this.GUIButtonEventHandler;
        }
        OnInitialize();
    }

    public void ControllerEnter(object onEnterParams)
    {
        if (GUIPanel != null && AutoGUISetActive == true)
        {
            SetPanelActive(true);
        }
        OnEnter(onEnterParams);
    }

    public void ControllerUpdate()
    {
        OnUpdate();
    }

    public void ControllerLateUpdate()
    {
        OnLateUpdate();
    }

    public void ControllerLeave()
    {
        if (GUIPanel != null && AutoGUISetActive == true)
        {
            SetPanelActive(false);
        }
        OnLeave();
    }

    public void ControllerOnGUI()
    {
        OnGUICustom();
    }

    /// <summary>
    /// Method for forwarding GUI button presses sent to OnButton of the panel declaration component.
    /// </summary>
    private void GUIButtonEventHandler(GameObject sender)
    {
        if (State.IsCurrentlyInState == true)
        {
            OnGUIButton(sender);
        }
    }

    //Overridable methods.
    protected virtual void OnInitialize() { }
    protected virtual void OnEnter(object onEnterParams) { }
    protected virtual void OnLeave() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnLateUpdate() { }
    protected virtual void OnGUICustom() { }
    protected virtual void OnGUIButton(GameObject sender) { }

    //Helper method.
    protected void ChangeState(string newStateName, object onEnterParam = null)
    {
        FsmSystem.ChangeState(newStateName, onEnterParam);
    }

    private void SetPanelActive(bool flag)
    {
#if UNITY_3_5
	  GUIPanel.SetActiveRecursively(flag);
#else
        LegacyTools.SetActiveRecursively(GUIPanel, flag);
#endif
    }
}
