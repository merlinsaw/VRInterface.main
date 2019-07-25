//-------------------------------------------------------
//	
//-------------------------------------------------------

#region using

using UnityEngine;

#endregion

/// <summary>
/// Helper class to simplify the access to common gameObjects in the scene.
/// </summary>
public class SceneHierarchy : MonoBehaviour
{
    public static readonly string pathStateControllers = "/game_logic/gamestates";

    //Single player paths.
    public static readonly string pathSinglePlayerScene = "/single_player_scene";
    public static readonly string pathLevelLogic = "/single_player_scene/level_logic";
    public static readonly string pathLevelAssets = "/single_player_scene/level";
}

