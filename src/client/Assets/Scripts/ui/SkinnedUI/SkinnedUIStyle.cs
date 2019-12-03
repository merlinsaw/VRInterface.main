using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


namespace Skinned.UI {
  [CreateAssetMenu(menuName = "Custom UI/NEW UI Style Data")]
  public class SkinnedUIStyle : ScriptableObject {

	public Sprite buttonSprite;
	public SpriteState buttonSpriteState;


	public Color defaultColor;
	public Sprite defaultIcon;

	public Color confirmColor;
	public Sprite confirmIcon;

	public Color declineColor;
	public Sprite declineIcon;

	public Color warningColor;
	public Sprite warningIcon;

  }
}