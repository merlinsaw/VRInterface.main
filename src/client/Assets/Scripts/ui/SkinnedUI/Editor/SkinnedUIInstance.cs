using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace Skinned.UI {
  public class SkinnedUIInstance : Editor{

  [MenuItem("GameObject/Skinned UI/Skinned Button", priority =0)]
  public static void AddButton() {
	Create("Button");
  }


  static GameObject currentSelectedObject;

  private static GameObject Create(string objectName) {
	GameObject instance = Instantiate(Resources.Load<GameObject>(objectName));
	instance.name = objectName;
	currentSelectedObject = UnityEditor.Selection.activeObject as GameObject;
	if (currentSelectedObject != null) {
	  instance.transform.SetParent(currentSelectedObject.transform, false);
	}
	return instance;
  }
  }
}
