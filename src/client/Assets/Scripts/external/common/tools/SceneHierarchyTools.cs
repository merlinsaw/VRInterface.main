//-------------------------------------------------------
//	
//-------------------------------------------------------

#region using

using System.Collections.Generic;
using UnityEngine;

#endregion

/// <summary>
/// Helper class to simplify the access to common gameObjects in the scene.
/// </summary>
public sealed class SceneHierarchyTools
{

  private static readonly _Logger log = new _Logger(typeof(SceneHierarchyTools));

  /// <summary>
  /// Method to return a gameObject called 'name' that is a child somewhere down in the hierarchy starting at root.
  /// NOTE: This method includes the root as possible result.
  /// </summary>
  public static GameObject FindChild(GameObject root, string name)
  {

	return (FindChildExtended(root, name, true));
  }

  /// <summary>
  /// Method to return a gameObject called 'name' that is a child somewhere down in the hierarchy starting at root.
  /// This version allows to specify whether or not the root should be included in the result.
  /// </summary>
  public static GameObject FindChildExtended(GameObject root, string name, bool includeRootAsPossibleResult)
  {

	List<GameObject> children = FindChildrenExtended(root, name, includeRootAsPossibleResult);
	if (children != null && children.Count > 0)
	{
	  return (children[0]);
	}
	return null;
  }

  public static List<GameObject> FindChildrenExtended(GameObject root, string name, bool includeRootAsPossibleResult)
  {

	List<GameObject> children = new List<GameObject>();

	if (root == null)
	{
	  log.Warn(_Logger.User.Msaw, "FindChildren failed. Given root node is null. Returning null.");
	  return (children);
	}

	//Better version using a stack instead of a list. Hopefully this structure is better optimized as it is exactly what we need here.
	Stack<Transform> transformStack = new Stack<Transform>();
	transformStack.Push(root.transform);

	while (transformStack.Count > 0)
	{
	  Transform transform = transformStack.Pop();
	  GameObject transformGO = transform.gameObject;

	  if (transformGO != null)
	  {
		if (transformGO.name == name)
		{
		  if (includeRootAsPossibleResult == true || transformGO != root)
		  {
			children.Add(transformGO);
		  }
		}
	  }

	  for (int i = 0; i < transform.childCount; ++i)
	  {
		transformStack.Push(transform.GetChild(i));
	  }
	}

	return (children);
  }

  /// <summary>
  /// Sets the active state for all children of the given game object
  /// </summary>
  public static void SetAllChildrenActive(GameObject root, bool active)
  {
	Transform rootTransform = root.transform;
	for (int i = 0; i < rootTransform.childCount; ++i)
	{
	  rootTransform.GetChild(i).gameObject.SetActive(active);
	}
  }

  /// <summary>
  /// Destroys all child transforms of a GameObject's transform
  /// </summary>
  public static void RemoveAllChildren(GameObject gameObject)
  {
	Transform rootTransform = gameObject.transform;
	for (int i = 0; i < rootTransform.childCount; ++i)
	{
	  GameObject.Destroy(rootTransform.GetChild(i).gameObject);
	}
  }

  /// <summary>
  /// Method to find the component of type T as child of the given scenePathBase in the hierarchy.
  /// This method also searches for the component in non-activated gameobjects.
  /// </summary>
  public static T GetComponentInChildren<T>(string scenePathBase) where T : Component
  {
	T[] panelDeclarations = GameObject.Find(scenePathBase).GetComponentsInChildren<T>(true);
	if (panelDeclarations != null && panelDeclarations.Length > 0)
	{
	  return (panelDeclarations[0]);
	}
	else
	{
	  log.Error(_Logger.User.Msaw, "Unable to find component '" + typeof(T) + "' as child within '" + scenePathBase + "'");
	}
	return (null);
  }

  /// <summary>
  /// Sets the layer for all children
  /// </summary>
  public static void SetLayerForAllChildren(GameObject root, int layer)
  {
	Transform rootTransform = root.transform;
	root.layer = layer;
	for (int i = 0; i < rootTransform.childCount; ++i)
	{
	  Transform child = rootTransform.GetChild(i);
	  child.gameObject.layer = layer;
	  SetLayerForAllChildren(child.gameObject, layer);
	}
  }
}
