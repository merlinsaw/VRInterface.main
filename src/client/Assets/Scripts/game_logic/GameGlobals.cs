    //-------------------------------------------------------
//	
//-------------------------------------------------------

#region using

using UnityEngine;

#endregion

public sealed class GameGlobals
{

    public static string AppVersion
    {
        get
        {
#if UNITY_EDITOR
            if (BuildInfo.Instance != null)
            {
                return BuildInfo.Instance.bundleVersion;
            }
            // use this in case BuildInfo is not available
            return "1.0.0";
#else
      if (BuildInfo.Instance != null) {
        return BuildInfo.Instance.bundleVersion;
      }

      // use this in case BuildInfo is not available
      return "1.0.0";
#endif
        }
    }

    public static string PlatformId
    {
        get
        {
            string platformId = Application.platform.ToString();
            return (platformId);
        }
    }

    public static bool isLogFileEnabled = true;



}