//-------------------------------------------------------
//	
//-------------------------------------------------------

#region using

using System.Collections.Generic;
using UnityEngine;

#endregion


public class TennisRacketPrefabs : MonoBehaviour
{

    protected static readonly _Logger log = new _Logger(typeof(TennisRacketPrefabs));

    public GameObject defaultRacketPrefab;

    public List<TennisRacketPrefabConfig> racketPrefabConfigs;

    public GameObject GetPrefabForWeaponWithId(string racketId)
    {
        TennisRacketPrefabConfig racketPrefabConfig = racketPrefabConfigs.Find(config => config.racketId == racketId);
        if (racketPrefabConfig != null)
        {
            return racketPrefabConfig.racketPrefab;
        }
        else
        {
            log.Warn(_Logger.User.Msaw, "No weapon prefab found for weapon with id " + racketId + ", returning default weapon prefab");

            if (defaultRacketPrefab == null)
            {
                log.Error(_Logger.User.Msaw, "No default weapon prefab found, returning null");
            }
            return defaultRacketPrefab;
        }
    }

    public void ValidateConfiguration()
    {

    }
}

