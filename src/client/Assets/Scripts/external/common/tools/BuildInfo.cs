//-------------------------------------------------------
//	
//-------------------------------------------------------

#region using

using UnityEngine;

#endregion

public class BuildInfo : MonoBehaviour
{
    public string bundleVersion;
    public string subversionRevision;
    public string buildConfiguration;
    public string environment;

    private static BuildInfo instance;
    public static BuildInfo Instance
    {
        get
        {
            return instance;
        }
    }

    public void Awake()
    {
        if (instance == null)
        {
            CreateInstance();
        }
    }

    public void CreateInstance()
    {
        DontDestroyOnLoad(gameObject);
        name = "build_info";
        instance = this;
    }
}

