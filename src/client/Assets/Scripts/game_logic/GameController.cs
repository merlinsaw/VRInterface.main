//-------------------------------------------------------
//	
//-------------------------------------------------------

#region using


#if UNITY_EDITOR
//using Newtonsoft.Json;
#endif
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using shared_data_layer;

using System;

using UI;


#endregion


/// <summary>
/// Class to manage and allow access to global game settings & logic.
/// </summary>
public sealed class GameController : MonoBehaviour
{

#pragma warning disable 414
    private static readonly _Logger log = new _Logger(typeof(GameController));
#pragma warning restore 414

    private static GameController instance = null;
    public static GameController Instance
    {
        get { return (instance); }
        private set { instance = value; }
    }


    // // private LevelController levelController = null;
    // // public LevelController LevelController
    // // {
    //	//get
    //	//{
    //	//  if (levelController == null)
    //	//  {
    //	//	GameObject goLevelLogic = GameObject.Find(SceneHierarchy.pathLevelLogic);
    //	//	if (goLevelLogic != null)
    //	//	{
    //	//	  levelController = goLevelLogic.GetComponentInChildren<LevelController>();
    //	//	}
    //	//  }
    //	//  return (levelController);
    //	//}
    // // }

    public FSMSystem FsmSystem { get; private set; }

    //#pragma warning disable 414
    //  private string currentFsmStateName = ""; //This helper variable is here to be able to view the current state in the unity editor. In order to do this, however, the unity inspector settings need to be set to debug mode to be able to view private variables.
    //#pragma warning restore 414

      #region User-Properties

      public User User { get; private set; }
    //  public string UserId { get { return (LoginController.UserId); } }

      #endregion

    //  private Animator CameraAnimation { get; set; }
    //  private Vector3 CameraScenePos { get; set; }



    //  public BuildInfo buildInfo;

    //  public GUISkin guiSkinNotFancy;

    // public PanelMainView PanelMainView { get; private set; }
    public PanelMainMenu PanelMainMenu { get; private set; }
    //  public PanelMatch PanelMatch { get; private set; }
    //  public PanelMatchFinished PanelMatchFinished { get; private set; }

    //  private PanelInfoErrorDialog PanelInfoErrorDialog { get; set; }
    //  private PanelDescriptionPopup PanelDescriptionPopup { get; set; }

    //  public FileDownloadManager FileDownloadManager { get; private set; }
    //  private LocaUpdateManager LocaUpdateManager { get; set; }

      private bool IsDebugMenuEnabled { get; set; }

    //  public GameCharacterRoot characterUserRoot = null;
    //  public GameCharacterRoot characterEnemyRoot = null;
    //  public GameCharacterRoot CharacterUserRoot { get { return (characterUserRoot); } }
    //  public GameCharacterRoot CharacterEnemyRoot { get { return (characterEnemyRoot); } }
    //  public GameCharacter CharacterUser { get { return (characterUserRoot.gameCharacter); } }
    //  public GameCharacter CharacterEnemy { get { return (characterEnemyRoot.gameCharacter); } }
    //  public TennisBall TennisBall;

    //  public GameStatsController GameStatsController { get; private set; }




    //#if UNITY_EDITOR
    //  public List<string> deviceNamesLocalServer;
    //#endif

    //  public Camera cameraMain;
    //  public Camera cameraMainUI;

    //  public SpawnPointController spawnPointController;
    //  public SpawnPointController SpawnPointController
    //  {
    //	get
    //	{
    //	  return spawnPointController;
    //	}
    //  }

    public TennisRacketPrefabs racketPrefabs;
    public TennisRacketPrefabs RacketPrefabs
    {
        get
        {
            return racketPrefabs;
        }
    }

    //  public UIWidget widgetOutOfScreen;

    //  public GameObject prefabPanelConfirmDialog;

    //  #region timing
    //  public float infoDisplayTimeInSeconds = 3.0f;
    //  #endregion

    #region colors
    public ColorManager colorManager;
    public ColorManager ColorManager
    {
        get { return colorManager; }
        private set { colorManager = value; }
    }
    #endregion


    #region icons
    public IconManager iconManager;
    public IconManager IconManager
    {
        get { return iconManager; }
        private set { iconManager = value; }
    }
    #endregion

    //  private static List<KeyValuePair<UILabel, string>> autoLocaLabels = new List<KeyValuePair<UILabel, string>>(); //label + localization code.
    //  private static List<KeyValuePair<UILabel, string>> AutoLocaLabels
    //  {
    //	get { return (autoLocaLabels); }
    //  }

    //  public GameStateMatchController GameStateMatchController { get; private set; }

    //  public bool IsInMainView { get { return (FsmSystem.CurrentStateName == "GameStateMainView"); } }

    //  static GameController()
    //  {
    //	UILabelLocalizer.OnAddLabelToAutoLocaList += AddLabelToAutoLocaList;
    //  }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            GameObject.DontDestroyOnLoad(this.transform.parent);
            Initialize();
            DebugAutoStartLevelIfSet();
        }
        else
        {
            GameObject.Destroy(this.transform.parent.gameObject); //Kill all duplicates that aren't the first instance.
            DebugAutoStartLevelIfSet();
        }
    }

    private void DebugAutoStartLevelIfSet()
    {
        //	if (string.IsNullOrEmpty(GameGlobals.debugStartLevelName) == false)
        //	{ //Auto start level in case a developer was starting a level inside unity.
        //	  DebugStartLevel(GameGlobals.debugStartLevelName);
        //	}
    }

    private void Initialize()
    {
        //	buildInfo.CreateInstance();

        _Logger.enableLogFile = GameGlobals.isLogFileEnabled;

        User = new User();
        //	GameStatsController = new GameStatsController();
        //	FileDownloadManager = GameObject.Find(SceneHierarchy.pathFileDownloadManager).GetComponent<FileDownloadManager>();
        //	LocaUpdateManager = new LocaUpdateManager();


        IsDebugMenuEnabled = false;

        //	if (buildInfo != null) {
        //	  if (EnvironmentEndpoints.TryGetValue(buildInfo.environment, out endpoint) == false) {
        //		endpoint = EnvironmentEndpoints[GameGlobals.environmentKeyDevelopment];
        //	  }
        //	} else {
        //	  log.Warn(Logger.User.Msaw, "BuildInfo.Instance is null, using fallback environment Development");
        //	}
        //	WSClient.ServerBaseUrl = endpoint;
        //	log.Debug(Logger.User.Msaw, "Using endpoint: " + WSClient.ServerBaseUrl);


        //	Webservices.Instance.ServerUrl = WSClient.ServerBaseUrl;
        //	Webservices.Instance.ErrorTokenExpired = ErrorMessages.tokenExpired.State;
        //	Webservices.Instance.OnTokenExpired += new System.Action(OnTokenExpired);

        //	PanelInfoErrorDialog = GetPanelDeclaration<PanelInfoErrorDialog>();
        //	PanelInfoErrorDialog.Initialize();

        //	List<AbstractFSMState> fsmStates = new List<AbstractFSMState>();
        //PanelMainView = GetPanelDeclaration<PanelMainView>();
        PanelMainMenu = GetPanelDeclaration<PanelMainMenu>();
        //	PanelMatch = GetPanelDeclaration<PanelMatch>();
        //	PanelMatchFinished = GetPanelDeclaration<PanelMatchFinished>();
        //	PanelDescriptionPopup = GetPanelDeclaration<PanelDescriptionPopup>();
        //	PanelIntro panelIntro = GetPanelDeclaration<PanelIntro>();
        //	GameStateMatchController = GetStateController<GameStateMatchController>();
        //	PanelDefaultMenuBackground panelDefaultMenuBackground = GetPanelDeclaration<PanelDefaultMenuBackground>();
        //	fsmStates.Add(new GameStateStartupGame("GameStateStartupGame", panelIntro));
        //	fsmStates.Add(new GameStateMainView("GameStateMainView", GetStateController<GameStateMainViewController>(), PanelMainView, PanelCashInfo, PanelMainView3d));
        //	fsmStates.Add(new GameStateMatch("GameStateMatch", GameStateMatchController, GetPanelDeclaration<PanelRoundFinished>(), PanelMatch, GetPanelDeclaration<PanelMatchTopHud>()));
        //	fsmStates.Add(new GameStateVerifyMatch("GameStateVerifyMatch"));
        //	fsmStates.Add(new GameStateMatchFinished("GameStateMatchFinished", PanelMatchFinished));
        //	FsmSystem = FSMSystem.CreateFSM("game_states", fsmStates, "");

        //	GameStatsController.OnGameEventReported += new GameStatsController.GameEventReported(OnGameEventReported);

        //	CharacterEnemy.IsHeadSideView = true; //Atm enemies are never seen from the front.

        //	// hide characters at start
        //	CharacterUserRoot.IsVisible = false;
        //	CharacterEnemyRoot.IsVisible = false;
        //  }

        //  public void OnGameEventReported(AbstractGameEvent gameEvent)
        //  {
        //	switch (gameEvent.GameEventType)
        //	{
        //	case GameEventType.GameDataReceived:
        //#if UNITY_EDITOR
        //	  LocaKeyGenerator.GenerateLocaKeys((gameEvent as EventGameDataReceived).GameData.BalancingData);
        //#endif
        //	  break;
        //	case GameEventType.SinglePlayerLevelEntered:
        //	  EnableMainInterface(false);
        //	  break;
        //	case GameEventType.SinglePlayerLevelLeft:
        //	  EnableMainInterface(true);
        //	  break;
        //	}
        //  }

        //  public void Update()
        //  {

        //	LocaUpdateManager.Update();

        //	//Store current state name in variable that may be seen in the debug mode of the inspector. This is for debugging purpose only.
        //	currentFsmStateName = FsmSystem.CurrentStateName;
        //  }


        //  /// <summary>
        //  /// Method to retrieve the server side player stats.
        //  /// </summary>
        //  public void RequestPlayerStats()
        //  {

        //	log.Error(Logger.User.Msaw, "RequestPlayerStats() is currently not implemented due to backend system change.");

        //	//if (GameController.Instance.IsUserRegistered == true) {
        //	//  Webservices.Instance.SendRequest<WSResponsePlayerGetStats>(new WSRequestPlayerGetStats(PlayerId.ToString()), this);
        //	//}
        //  }

        /// <summary>
        /// Helper method to reduce amount of code necessary to find a panel declaration component in the scene.
        /// </summary>
        
        //public static
        T GetPanelDeclaration<T>() where T : AbstractPanelDeclarations
        {
            return (SceneHierarchyTools.GetComponentInChildren<T>(SceneHierarchy.pathUiCameraLevel));
        }
        

        //  /// <summary>
        //  /// Helper method to reduce amount of code necessary to find a gamestate controller component in the scene.
        //  /// </summary>
        //  public static T GetStateController<T>() where T : AbstractFSMStateController
        //  {
        //	return (SceneHierarchyTools.GetComponentInChildren<T>(SceneHierarchy.pathStateControllers));
        //  }

        //  /// <summary>
        //  /// Method that allows to change to the login scene from wherever it is needed.
        //  /// </summary>
        //  public void ChangeStateToLogin()
        //  {
        //	FsmSystem.ChangeState("GameStateReadyForLogin");
        //  }



        //  /// <summary>
        //  /// GUI helper compatibility method.
        //  /// </summary>
        //  public static void AddLabelToAutoLocaList(UILabel label, string locaCode)
        //  {
        //	if (label != null)
        //	{
        //	  AutoLocaLabels.Add(new KeyValuePair<UILabel, string>(label, locaCode)); //Store label in case loca isn't ready yet and also for being able to change the text in case the game language is changed.
        //																			  // TODO: check if we have a situation where loca isn't ready yet
        //																			  //if (Loca == true) {
        //	  label.text = Loca.Get(locaCode);
        //	  //}
        //	}
        //  }

        //  /// <summary>
        //  /// Helper method to localize all registered labels according to the latest loca info.
        //  /// </summary>
        //  public static void PerformAutoLocalization()
        //  {
        //	int numRemovedItems = AutoLocaLabels.RemoveAll(item => item.Key == null); //Remove all labels that have become invalid (e.g. that have been deleted at runtime at some point).
        //	if (numRemovedItems > 0)
        //	{
        //	  log.InfoMS("Removed " + numRemovedItems + " label(s) from auto loca list.");
        //	}

        //	foreach (KeyValuePair<UILabel, string> kvp in AutoLocaLabels)
        //	{
        //	  if (kvp.Key != null)
        //	  {
        //		kvp.Key.text = Loca.Get(kvp.Value);
        //	  }
        //	  else
        //	  {
        //		log.Warn(Logger.User.Msaw, "Cannot apply localized text to label, because label is null. Text: " + Loca.Get(kvp.Value));
        //	  }
        //	}
        //  }



        /// <summary>
        /// Updates player data such as: equipment etc.
        /// </summary>
        //public
        void RequestPlayerUpdate()
        {

        }
        



        //  private void DebugStartLevel(string levelFullName)
        //  {
        //	//log.InfoMS("StartLevel - '" + levelFullName + "'");
        //	levelController = null; //Invalidate the level controller so it gets reassigned with the one from the newly loaded level.
        //	SceneFSM.SwitchState(levelFullName);
        //  }

        //  public void EnableMainInterface(bool enabled)
        //  {
        //	if (cameraMain != null)
        //	{
        //	  cameraMain.gameObject.SetActive(enabled);
        //	}
        //	//if (cameraMainUI != null) {
        //	//  cameraMainUI.gameObject.SetActive(enabled);
        //	//}
        //  }

        //  /// <summary>
        //  /// Method to load single-player scene setup in background. The given Action is invoked when the loading has completed.
        //  /// </summary>
        //  public void LoadSinglePlayerScene(Action sceneLoadedCallback)
        //  {
        //	StartCoroutine(LoadSinglePlayerSceneCoroutine(sceneLoadedCallback));
        //  }

        //  /// <summary>
        //  /// Internal single player scene loading method.
        //  /// </summary>
        //  private IEnumerator LoadSinglePlayerSceneCoroutine(Action sceneLoadedCallback)
        //  {
        //	log.DebugMS("LoadSinglePlayerSceneCoroutine() - Start");
        //	while (FileDownloadManager.IsDownloadInProgress == true)
        //	{ //This is an attempt to find the crash bug that happened around 12.12.2014. The bug appeared to occur in case a user went to the surface too quickly after the launch of the app. Maybe the reason is some kind of interference with downloads still in progress.
        //	  yield return null;
        //	}
        //	yield return Application.LoadLevelAdditiveAsync(GameGlobals.sceneNameSinglePlayer);
        //	if (sceneLoadedCallback != null)
        //	{
        //	  sceneLoadedCallback.Invoke();
        //	}
        //	log.DebugMS("LoadSinglePlayerSceneCoroutine() - Finish");
        //  }



        //  /// <summary>
        //  /// Method to set up and display a description popup, e.g. used for the item features description
        //  /// </summary>
        //  /// <param name="localizedHeader"></param>
        //  /// <param name="localizedDescription"></param>
        //  /// <param name="confirmAction"></param>
        //  public void ShowPopupPanelDescription(string localizedHeader, string localizedDescription, string localizedConfirmButton, Action confirmAction)
        //  {
        //	PanelDescriptionPopup.SetupPanel(localizedHeader, localizedDescription, localizedConfirmButton, confirmAction);
        //	GUITools.SetActive(PanelDescriptionPopup.gameObject, true);
        //  }

        //  public void ShowPopupPanelGetMoreHardCash()
        //  {
        //	log.DebugMS("ShowPopupPanelGetMoreHardCash()");
        //	PanelDescriptionPopup.SetupPanelGetMoreHardCash();
        //	GUITools.SetActive(PanelDescriptionPopup.gameObject, true);
        //  }

        //  /// <summary>
        //  /// Method to display an info- or error-text 
        //  /// </summary>
        //  public void ShowInfoErrorDialog(string locaString, DialogueType dialogueType = DialogueType.Info)
        //  {
        //	string text = Loca.Get(locaString);
        //	ShowInfoErrorDialogUnlocalized(text, dialogueType);
        //  }

        //  /// <summary>
        //  /// Method to display an info- or error-text 
        //  /// </summary>
        //  public void ShowInfoErrorDialogUnlocalized(string text, DialogueType dialogueType = DialogueType.Info)
        //  {
        //	Color textColor = Color.cyan;
        //	switch (dialogueType)
        //	{
        //	case DialogueType.Info:
        //	  textColor = ColorManager.DefaultInfoTextColor;
        //	  break;
        //	case DialogueType.Error:
        //	  textColor = ColorManager.DefaultErrorTextColor;
        //	  break;
        //	default:
        //	  log.Error(Logger.User.Msaw, "Failed to fetch default color for dialogueType: " + dialogueType);
        //	  break;
        //	}
        //	//float displayTimeParam = infoDisplayTimeInSeconds;
        //	ShowInfoErrorDialog(text, dialogueType, textColor, infoDisplayTimeInSeconds);
        //  }

        //  /// <summary>
        //  /// Method to display an info- or error-text with custom color and time.
        //  /// </summary>
        //  private void ShowInfoErrorDialog(string text, DialogueType dialogueType, Color textColorParam, float displayTimeParam)
        //  {
        //	Color textColor = textColorParam;
        //	float displayTime = displayTimeParam;

        //	PanelInfoErrorDialog.SetupAndShowMessageUnlocalized(text, textColor, displayTime);
        //  }


        //  public Sprite LoadSkillIcon(string skillId)
        //  {
        //	return Resources.Load<UnityEngine.Sprite>("skill_icons/skill_icon_" + skillId.ToUpper());
        //  }

        //  public Sprite LoadSkillIconDisabled(string skillId)
        //  {
        //	return Resources.Load<UnityEngine.Sprite>("skill_icons/skill_icon_" + skillId.ToUpper() + "_disabled");
        //  }

        //  private void ChangeToMainViewCommon()
        //  {
        //	if (LevelController != null && LevelController.IsLevelStarted == true)
        //	{
        //	  LevelController.LeaveSinglePlayer();
        //	}

        //	// hide enemy
        //	CharacterEnemyRoot.IsVisible = false;
        //	CharacterEnemyRoot.IsDefenseTurretVisible = false;
        //  }

        //  /// <summary>
        //  /// Method to switch back to the main view. To be invoked in case of an error for example.
        //  /// </summary>
        //  public void ChangeToMainView()
        //  {
        //	ChangeToMainViewCommon();
        //	log.DebugMS("ChangeToMainView()");
        //	FsmSystem.ChangeState("GameStateMainView");
        //  }

        //  /// <summary>
        //  /// Creates a confirm dialog and adds it to the main UI
        //  /// </summary>
        //  public PanelConfirmDialog CreateConfirmDialog()
        //  {
        //	GameObject go = GUITools.AddChild(cameraMainUI.gameObject, prefabPanelConfirmDialog);
        //	return go.GetComponent<PanelConfirmDialog>();
        //  }



    }
}
