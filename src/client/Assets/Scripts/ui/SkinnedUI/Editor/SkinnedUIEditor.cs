//-------------------------------------------------------
//	
//-------------------------------------------------------

#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEditor;

#endregion

namespace Skinned.UI
{
  [CustomEditor(typeof(SkinnedUIButton))]
  public class SkinnedUIEditor : Editor
  {
	public override void OnInspectorGUI()
	{
	  DrawDefaultInspector();

	  SkinnedUIButton skinnedUI = (SkinnedUIButton)target;
	  if (GUILayout.Button("Update UI Skin"))
	  {
		skinnedUI.UpdateSkinUI();
		Debug.Log("Skin Update done.");
	  }
	}
  }
}
