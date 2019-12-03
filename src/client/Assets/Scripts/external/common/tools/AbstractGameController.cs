//-------------------------------------------------------
//	
//-------------------------------------------------------

#region using

using System.Collections.Generic;
using UnityEngine;

#endregion

/// <summary>
/// Base class for a typical GameController
/// </summary>
public abstract class AbstractGameController<T> : MonoBehaviour where T : class
{

    #region Inspector Elements

    public string sceneHierarchyPathUi = "/UI Root";
    public string sceneHierarchyPathStateControllers = "/game_logic/state_controllers";
    public string sceneHierarchyPathGameLogic = "/game_logic/";
    public string sceneHierarchyPathScene = "/scene/";

    #endregion

    public string SceneHierarchyPathUi
    {
        get
        {
            return sceneHierarchyPathUi;
        }
    }

    public string SceneHierarchyPathStateControllers
    {
        get
        {
            return sceneHierarchyPathStateControllers;
        }
    }

    public string SceneHierarchyPathGameLogic
    {
        get
        {
            return sceneHierarchyPathGameLogic;
        }
    }

    public string SceneHierarchyPathScene
    {
        get
        {
            return sceneHierarchyPathScene;
        }
    }

    protected static readonly _Logger log = new _Logger(typeof(AbstractGameController<T>));

    public BuildInfo buildInfo;
    public BuildInfo BuildInfo
    {
        get
        {
            if (buildInfo == null)
            {
                log.Warn(_Logger.User.Msaw, "BuildInfo not found, creating an empty one");
                GameObject goBuildInfo = new GameObject();
                buildInfo = goBuildInfo.AddComponent<BuildInfo>();
                buildInfo.subversionRevision = "unknown";
                buildInfo.bundleVersion = "1.0";
            }
            return buildInfo;
        }
    }

    public static T Instance
    {
        get
        {
            return instance;
        }
    }
    private static T instance;

    protected abstract string StartupGameState { get; }

    public FSMSystem FsmSystem { get; private set; }

    public List<SystemLanguage> supportedLanguages = new List<SystemLanguage>();

    public string Locale { get; private set; }

    public string editorLocale = "en-US";

    public void Awake()
    {
        instance = this as T;
        log.Info(_Logger.User.Msaw, "Initializing game controller...");

        // default locale
        Locale = "en-us";

#if UNITY_EDITOR
        SetLanguage(editorLocale);
#endif

        OnAwake();

        List<AbstractFSMState> states = CreateGameStates();
        log.Info(_Logger.User.Msaw, "Starting up with " + StartupGameState);

        FsmSystem = FSMSystem.CreateFSM("game", states, StartupGameState);
    }

    public void SetLanguage(string language)
    {
        log.Info(_Logger.User.Msaw, "Setting language: " + language);
        SystemLanguage systemLanguage = SystemLanguage.English;

        if (string.IsNullOrEmpty(language) == false || language.Length < 2)
        {
            Locale = language.ToLower();
            language = language.ToLower().Substring(0, 2);
            systemLanguage = LanguageHelper.GetSystemLanguage(language);
            InitializeLoca(systemLanguage);
        }
        else
        {
            log.Error(_Logger.User.Msaw, "Using fallback language, given one is invalid: " + language);
            Locale = "en-US";
        }
    }

    private void InitializeLoca(SystemLanguage systemLanguage)
    {
        Loca.SupportedLanguages = supportedLanguages;
        Loca.CurrentLanguage = systemLanguage;
        //UILabelLocalizer.OnAddLabelToAutoLocaList += OnAddLabelToAutoLocaList;
    }

    /*
    private void OnAddLabelToAutoLocaList(UILabel label, string locaCode)
    {

        log.DebugMS("OnAddLabelToAutoLocaList - locaCode: " + locaCode);
        // translate directly, no language switching supported anyway
        label.text = Loca.Get(locaCode);
    }
    */
    public void ChangeState(string stateName, object onEnterParams = null)
    {
        FsmSystem.ChangeState(stateName, onEnterParams);
    }

    /// <summary>
    /// Helper method to reduce amount of code necessary to find a panel declaration component in the scene.
    /// </summary>
    public PanelDeclarationsType GetPanelDeclaration<PanelDeclarationsType>() where PanelDeclarationsType : AbstractPanelDeclarations
    {
        return (SceneHierarchyTools.GetComponentInChildren<PanelDeclarationsType>(sceneHierarchyPathUi));
    }

    /// <summary>
    /// Helper method to get a specific state controller
    /// </summary>
    public FSMStateControllerType GetStateController<FSMStateControllerType>() where FSMStateControllerType : AbstractFSMStateController
    {
        return (SceneHierarchyTools.GetComponentInChildren<FSMStateControllerType>(sceneHierarchyPathStateControllers));
    }

    protected abstract void OnAwake();
    protected abstract List<AbstractFSMState> CreateGameStates();
}
