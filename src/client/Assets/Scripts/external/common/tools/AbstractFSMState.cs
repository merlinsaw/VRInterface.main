using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Class to define the interface of a state of a finite state machine.
/// </summary>
public abstract class AbstractFSMState
{

    protected static readonly _Logger log = new _Logger(typeof(AbstractFSMState));

    protected FSMSystem FsmSystem { get; set; }
    public string StateName { get; protected set; }
    public AbstractFSMStateController StateController { get; protected set; }
    public List<AbstractPanelDeclarations> MultiPanelDeclarations { get; private set; } //Used in case there are multiple panels per state.
    public float TimeInState { get; private set; } //The time elapsed since the state was entered. Only valid if the state is active.

    public bool NeedsGUILayout
    {
        get;
        protected set;
    }

    public bool IsCurrentlyInState { get { return (FsmSystem.CurrentStateName == StateName); } } //To be used for example by webservice response handlers to find out if the state is still active or if it may have changed since the request start.

    public AbstractFSMState(string stateName)
    {
        StateName = stateName;
        StateController = null;
        MultiPanelDeclarations = new List<AbstractPanelDeclarations>();
    }

    public AbstractFSMState(string stateName, AbstractFSMStateController stateController)
    {
        StateName = stateName;
        StateController = stateController;
        MultiPanelDeclarations = new List<AbstractPanelDeclarations>();
    }

    public AbstractFSMState(string stateName, AbstractFSMStateController stateController, AbstractPanelDeclarations panelDeclarations)
    {
        StateName = stateName;
        StateController = stateController;
        MultiPanelDeclarations = new List<AbstractPanelDeclarations>();
        if (panelDeclarations != null)
        {
            MultiPanelDeclarations.Add(panelDeclarations);
        }
    }

    public AbstractFSMState(string stateName, AbstractPanelDeclarations panelDeclarations)
    {
        StateName = stateName;
        StateController = null;
        MultiPanelDeclarations = new List<AbstractPanelDeclarations>();
        if (panelDeclarations != null)
        {
            MultiPanelDeclarations.Add(panelDeclarations);
        }
    }

    public AbstractFSMState(string stateName, List<AbstractPanelDeclarations> panelDeclarations, AbstractFSMStateController stateController)
    {
        StateName = stateName;
        StateController = stateController;
        MultiPanelDeclarations = new List<AbstractPanelDeclarations>();
        if (panelDeclarations != null && panelDeclarations.Count > 0)
        {
            MultiPanelDeclarations.AddRange(panelDeclarations);
            MultiPanelDeclarations.RemoveAll(item => item == null); //Pure sanity check.
        }
    }

    public AbstractFSMState(string stateName, List<AbstractPanelDeclarations> panelDeclarations)
    {
        StateName = stateName;
        StateController = null;
        MultiPanelDeclarations = new List<AbstractPanelDeclarations>();
        if (panelDeclarations != null && panelDeclarations.Count > 0)
        {
            MultiPanelDeclarations.AddRange(panelDeclarations);
            MultiPanelDeclarations.RemoveAll(item => item == null); //Pure sanity check.
        }
    }

    public void Initialize(FSMSystem fsmSystem)
    {
        this.FsmSystem = fsmSystem;
        bool letControllerControlGui = (MultiPanelDeclarations.Count == 0); //Only when there are no panel declaration scripts, should the controller be in control of the GUI enabling/disabling.
        if (StateController != null)
        {
            AbstractPanelDeclarations primaryPanelDeclaration = null;
            if (MultiPanelDeclarations.Count > 0)
            {
                primaryPanelDeclaration = MultiPanelDeclarations[0];
            }
            StateController.ControllerInitialize(fsmSystem, this, primaryPanelDeclaration, letControllerControlGui);
        }
        if (MultiPanelDeclarations.Count > 0)
        {
            foreach (AbstractPanelDeclarations panelDeclarations in MultiPanelDeclarations)
            {
                panelDeclarations.OnButtonClicked += this.GUIButtonEventHandler;
                panelDeclarations.PanelInitialize(fsmSystem, !letControllerControlGui, this);
            }
        }
        NeedsGUILayout = true;
        TimeInState = 0;
        OnInitialize();
    }

    public void Enter(object onEnterParams)
    {
        MultiPanelDeclarations.ForEach(item => item.PanelEnter(onEnterParams));
        if (StateController != null)
        {
            StateController.ControllerEnter(onEnterParams);
        }
        TimeInState = 0;
        OnEnter(onEnterParams);
    }

    public void Update()
    {
        MultiPanelDeclarations.ForEach(item => item.PanelUpdate());
        if (StateController != null)
        {
            StateController.ControllerUpdate();
        }
        TimeInState += Time.deltaTime;
        OnUpdate();
    }

    public void LateUpdate()
    {
        MultiPanelDeclarations.ForEach(item => item.PanelLateUpdate());
        if (StateController != null)
        {
            StateController.ControllerLateUpdate();
        }
        OnLateUpdate();
    }

    public void DrawGUI()
    {
        MultiPanelDeclarations.ForEach(item => item.PanelOnGUI());
        if (StateController != null)
        {
            StateController.ControllerOnGUI();
        }
        OnGUI();
    }

    public void Leave(List<AbstractPanelDeclarations> panelsActiveInNextState)
    {
        MultiPanelDeclarations.ForEach(item => item.PanelLeave(panelsActiveInNextState.Contains(item)));
        if (StateController != null)
        {
            StateController.ControllerLeave();
        }
        OnLeave();
    }

    /// <summary>
    /// Method for forwarding GUI button presses sent to OnButton of the panel declaration component.
    /// </summary>
    private void GUIButtonEventHandler(GameObject sender)
    {
        if (IsCurrentlyInState == true)
        {
            OnGUIButton(sender);
        }
    }

    //Overridable methods.
    protected abstract void OnInitialize();
    protected abstract void OnEnter(object onEnterParams);
    protected virtual void OnUpdate() { }
    protected virtual void OnLateUpdate() { } //Not mandatory, but can be implemented if needed.
    protected virtual void OnGUI() { }
    protected abstract void OnLeave();
    protected virtual void OnGUIButton(GameObject sender) { }

    //Helper methods.
    protected void ChangeState(string newStateName, object onEnterParam = null)
    {
        FsmSystem.ChangeState(newStateName, onEnterParam);
    }
    protected void ChangeStateToNextInList(object onEnterParam = null)
    {
        FsmSystem.ChangeStateToNextInList(onEnterParam);
    }
    protected void ChangeStateToPrevious(object onEnterParam = null)
    {
        FsmSystem.ChangeState(FsmSystem.PreviousStateName);
    }

    /// <summary>
    /// Returns the first panel with the given type from MultiPanelDeclarations
    /// </summary>
    public T GetPanel<T>() where T : AbstractPanelDeclarations
    {
        foreach (AbstractPanelDeclarations panelDeclaration in MultiPanelDeclarations)
        {
            T panel = panelDeclaration as T;
            if (panel != null)
            {
                return panel;
            }
        }

        log.Error(_Logger.User.Msaw, "Panel of type " + typeof(T).ToString() + " does not exist in state " + StateName);

        return null;
    }

    /// <summary>
    /// Method that allows pure states, that aren't derived from a MonoBehaviour to
    /// run Coroutines through the FsmSystem.
    /// </summary>
    protected Coroutine StartCoroutine(IEnumerator routine)
    {
        return (FsmSystem.StartCoroutine(routine));
    }

    protected void StopAllCoroutines()
    {
        FsmSystem.StopAllCoroutines();
    }
}
