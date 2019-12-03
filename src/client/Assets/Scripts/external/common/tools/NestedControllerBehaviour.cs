using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NestedControllerBehaviour : MonoBehaviour, INestedController
{

    protected static _Logger log = new _Logger(typeof(NestedControllerBehaviour));

    public delegate void PrefabLoadDelegate<T>(T t);

    private List<GameObject> injectedGOs = new List<GameObject>();
    private List<GameObject> InjectedGOs
    {
        get { return injectedGOs; }
        set { injectedGOs = value; }
    }

    public void Inject(GameObject nestedGO)
    {
        if (InjectedGOs.Contains(nestedGO) == false)
        {
            //log.DebugJG("Inject: " + nestedGO);
            InjectedGOs.Add(nestedGO);
        }
    }

    protected T GetInjectedComponent<T>(string injectedPrefabName, bool suppressError = false) where T : Component
    {
        GameObject injectedGO = GetInjectedGO(injectedPrefabName, suppressError);
        if (injectedGO != null)
        {
            T injectedComponent = injectedGO.GetComponent<T>();
            return (injectedComponent);
        }
        else
        {
            if (suppressError == false)
            {
                log.Error(_Logger.User.Msaw, "Failed to find component of type '" + typeof(T) + "' on gameobject of name '" + injectedPrefabName + "'.");
            }
            return (null);
        }
    }

    protected GameObject GetInjectedGO(string injectedPrefabName, bool suppressError = false)
    {
        GameObject injectedGO = InjectedGOs.Find(item => item.name == injectedPrefabName);
        if (injectedGO == null)
        {
            if (suppressError == false)
            {
                log.Error(_Logger.User.Msaw, "Failed to find injected gameobject of name '" + injectedPrefabName + "'.");
            }
        }
        return (injectedGO);
    }

    #region syntatic sugar methods
    public void LoadNested<T>(IWrappedRef<T> refObject) where T : Component
    {
        LoadNestedComponentAsync<T>(refObject.NestedPrefabName, refObject);
    }
    protected void LoadNested<T>(string prefabName, IWrappedRef<T> refObject) where T : Component
    {
        LoadNestedComponentAsync<T>(prefabName, refObject);
    }
    protected void LoadNestedGO(string prefabName, IWrappedRef<GameObject> refObject)
    {
        LoadNestedGameObjectAsync(prefabName, refObject);
    }
    #endregion

    protected void LoadNestedComponentAsync<T>(string prefabName, IWrappedRef<T> refObject) where T : Component
    {
        if (refObject == null)
        {
            log.Error(_Logger.User.Msaw, "refObject == null");
        }
        refObject.Reference = GetInjectedComponent<T>(prefabName, true);
        if (refObject.Reference == null)
        {
            StartCoroutine(LoadNestedComponentCoroutine<T>(prefabName, refObject));
        }
    }

    protected void LoadNestedGameObjectAsync(string prefabName, IWrappedRef<GameObject> refObject)
    {
        if (refObject == null)
        {
            log.Error(_Logger.User.Msaw, "refObject == null");
        }
        refObject.Reference = GetInjectedGO(prefabName, true);
        if (refObject.Reference == null)
        {
            StartCoroutine(LoadNestedGameObjectCoroutine(prefabName, refObject));
        }
    }

    private IEnumerator LoadNestedComponentCoroutine<T>(string prefabName, IWrappedRef<T> refObject) where T : Component
    {
        refObject.Reference = GetInjectedComponent<T>(prefabName, true);
        while (refObject.Reference == null)
        {
            yield return null;
            refObject.Reference = GetInjectedComponent<T>(prefabName, true);
        }
        log.DebugMS("LoadNestedComponentCoroutine - success. injectedComponent: " + refObject.Reference + ", refObject.Value: " + refObject.Reference);
    }

    private IEnumerator LoadNestedGameObjectCoroutine(string prefabName, IWrappedRef<GameObject> refObject)
    {
        refObject.Reference = GetInjectedGO(prefabName);
        while (refObject.Reference == null)
        {
            yield return null;
            refObject.Reference = GetInjectedGO(prefabName);
        }
    }
}

