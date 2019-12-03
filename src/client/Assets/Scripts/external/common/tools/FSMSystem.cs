using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;


/// <summary>
/// Class to handle a finite state machine.
/// 
/// Usage:
/// 
///  List<AbstractFSMState> fsmStates = new List<AbstractFSMState>();
///  fsmStates.Add(new LevelStateMain("LevelStateMain"));
///  fsmStates.Add(new LevelStateSelectModifier("LevelStateSelectModifier"));
///  FsmSystem = FSMSystem.CreateFSM("level_state", fsmStates, "");
///  
/// </summary>
public sealed class FSMSystem : MonoBehaviour
{

    private static readonly _Logger log = new _Logger(typeof(FSMSystem));

    public static FSMSystem CreateFSM(string systemName, List<AbstractFSMState> states, string startupState)
    {

        if (states == null || states.Count == 0)
        {
            log.Error(_Logger.User.Msaw, "Cannot create FSMSystem '" + systemName + "' because given parameter states was null or empty. A valid list of states must be given.");
            return (null);
        }

        GameObject go = new GameObject("fsm_" + systemName);
        go.AddComponent<FSMSystem>();
        FSMSystem fsmSystem = go.GetComponent<FSMSystem>();
        fsmSystem.States = states;
        fsmSystem.CurrentState = states.Find(item => item.StateName == startupState);

        return (fsmSystem);
    }

    public string CurrentStateName
    {
        get
        {
            if (CurrentState == null)
            {
                return ("");
            }
            else
            {
                return (CurrentState.StateName);
            }
        }
    }

    public string PreviousStateName { get; set; }

    private List<AbstractFSMState> States { get; set; }
    private AbstractFSMState CurrentState { get; set; }
    private string RequestedNewState { get; set; }
    private object RequestedNewStateOnEnterParams { get; set; }

    public event System.Action<string> OnStateEnter; //Event to be invoked when a new state is entered. NOTE: This event is invoked before OnEnter of the state is called.

    public float TimeInCurrentState
    {
        get
        {
            if (CurrentState != null)
            {
                return CurrentState.TimeInState;
            }
            else
            {
                return 0.0f;
            }
        }
    }

    void Start()
    {
        PreviousStateName = "";
        if (CurrentState == null)
        {
            CurrentState = States[0];
        }
        foreach (AbstractFSMState fsmState in States)
        {
            fsmState.Initialize(this);
        }
        CurrentState.Enter(null);
    }

    void Update()
    {
        CurrentState.Update();
    }

    void LateUpdate()
    {
        CurrentState.LateUpdate();

        if (string.IsNullOrEmpty(RequestedNewState) == false)
        {
            PreviousStateName = CurrentState.StateName;
            AbstractFSMState newState = States.Find(item => item.StateName == RequestedNewState);
            CurrentState.Leave(newState.MultiPanelDeclarations);
            CurrentState = newState;
            RequestedNewState = "";
            if (OnStateEnter != null)
            {
                OnStateEnter(newState.StateName);
            }
            CurrentState.Enter(RequestedNewStateOnEnterParams);

            if (CurrentState.NeedsGUILayout)
            {
                useGUILayout = true;
            }
            else
            {
                useGUILayout = false;
            }
        }
    }

#if !DISABLE_ONGUI
    public void OnGUI()
    {
        CurrentState.DrawGUI();
    }
#endif

    public void ChangeState(string stateName, object onEnterParam = null)
    {

        //log.DebugME("ChangeState(" + stateName + ")");
        if (States.Exists(item => item.StateName == stateName) == false)
        {
            log.Error(_Logger.User.Msaw, "Cannot switch to given state '" + stateName + "'. It doesn't exist in the state machine system.");
            return;
        }
        RequestedNewState = stateName;
        RequestedNewStateOnEnterParams = onEnterParam;
    }

    /// <summary>
    /// Method to change to the next state defined in the states list. This corresponds to the order in which the states are added upon creating the fsm system.
    /// This method can be used to skip a sequence of states in order.
    /// </summary>
    public void ChangeStateToNextInList(object onEnterParam = null)
    {

        int currentStateIndex = States.IndexOf(CurrentState);
        log.DebugME("ChangeStateToNextInList - currentStateIndex = " + currentStateIndex);
        int nextStateIndex = currentStateIndex + 1;
        if (nextStateIndex < States.Count)
        {
            RequestedNewState = States[nextStateIndex].StateName;
            RequestedNewStateOnEnterParams = onEnterParam;
        }
        else
        {
            log.Warn(_Logger.User.Msaw, "Reached last state in list ('" + CurrentStateName + "'). Cannot change to next state.");
        }

    }

    /// <summary>
    /// Returns the state controller for the given state
    /// or the state controller for the current state if stateName is null
    /// </summary>
    public AbstractFSMStateController GetStateController(string stateName = null)
    {
        if (stateName == null)
        {
            stateName = CurrentStateName;
        }

        AbstractFSMState stateObject = States.Find(item => item.StateName == stateName);
        if (stateObject != null)
        {
            return (stateObject.StateController);
        }
        else
        {
            log.Error(_Logger.User.Msaw, "Cannot get state controller for state '" + stateName + "'. It doesn't exist in the state machine system. Returning null.");
        }
        return (null);
    }


}

