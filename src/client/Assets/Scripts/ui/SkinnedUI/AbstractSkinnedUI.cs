using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Skinned.UI
{

  [ExecuteInEditMode()]
  public abstract class AbstractSkinnedUI : MonoBehaviour
  {

	public SkinnedUIStyle skinnedUiStyle;

	protected virtual void OnSkinUI()
	{


	}

	protected virtual void Awake()
	{
	  OnSkinUI();
	}

	// used to call in custom editor
	public virtual void UpdateSkinUI()
	{
	  OnSkinUI();
	}

	// check if we are in editmode and update them while we are working on them.
	// TODO: more optimally write a custom editor script to call the update method so when the game is running we are not doing this check.
	public virtual void Update()
	{

	  if (Application.isEditor)
	  {
		OnSkinUI();
	  }
	}
  }

}

