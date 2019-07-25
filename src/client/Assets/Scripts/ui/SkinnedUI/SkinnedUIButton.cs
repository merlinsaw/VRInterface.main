using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using AdvancedUI;


namespace Skinned.UI {

  [RequireComponent(typeof(AdvancedButton))]
  [RequireComponent(typeof(Image))]
  [RequireComponent(typeof(BoxCollider))]
  [RequireComponent(typeof(Animator))]
  public class SkinnedUIButton : AbstractSkinnedUI {

	private static readonly _Logger log = new _Logger(typeof(SkinnedUIButton));

	protected Image image;
	protected Image icon;
	protected Text text;
	protected AdvancedButton button;
	protected BoxCollider hitbox;
	protected RectTransform rect;

	[SerializeField]
	protected bool DestroyOnAwake;
	public SkinnedUIButtonType buttonType;

	protected override void Awake() {

	  hitbox = GetComponent<BoxCollider>();
	  image = GetComponent<Image>();
	  icon = transform.Find("Icon").GetComponent<Image>();
	  text = transform.Find("Text").GetComponent<Text>();
	  button = GetComponent<AdvancedButton>();
	  rect = GetComponent<RectTransform>();

	  hitbox.isTrigger = true;

	  base.Awake();
	  if (DestroyOnAwake)
		Destroy(this);
	}

	protected override void OnSkinUI() {

	  hitbox.size = rect.sizeDelta;

	  //button.transition = Selectable.Transition.SpriteSwap;
	  //button.transition = Selectable.Transition.ColorTint;
	  button.transition = Selectable.Transition.Animation;
	  button.targetGraphic = image;

	  if (skinnedUiStyle == null) {
		log.Error(_Logger.User.Msaw, "Missing a (UI Style Data) Asset.\nCreate one using: Assets > Create > Custom UI > NEW UI Style Data.");
		EditorGUIUtility.PingObject(this);
		enabled = false;
	  } else {
		image.sprite = skinnedUiStyle.buttonSprite;
		// using sliced sprites for appropriate scaling
		image.type = Image.Type.Sliced;
		button.spriteState = skinnedUiStyle.buttonSpriteState;

		switch (buttonType) {
		case SkinnedUIButtonType.Confirm:
		  image.color = skinnedUiStyle.confirmColor;
		  icon.sprite = skinnedUiStyle.confirmIcon;
		  break;
		case SkinnedUIButtonType.Decline:
		  image.color = skinnedUiStyle.declineColor;
		  icon.sprite = skinnedUiStyle.declineIcon;
		  break;
		case SkinnedUIButtonType.Warning:
		  image.color = skinnedUiStyle.warningColor;
		  icon.sprite = skinnedUiStyle.warningIcon;
		  break;
		default:
		  image.color = skinnedUiStyle.defaultColor;
		  icon.sprite = skinnedUiStyle.defaultIcon;
		  break;
		}
	  }



	  base.OnSkinUI();
	}
  }

}