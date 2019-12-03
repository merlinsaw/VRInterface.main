////-------------------------------------------------------
////	
////-------------------------------------------------------

//#region using

//using UnityEngine;

//#endregion


//public class PanelConfirmDialog : MonoBehaviour
//{

//  #region GUI Elements

//  public UILabel labelHeader;
//  public UILabel labelBody;
//  public NGUIGenericButton buttonOk;
//  public NGUIGenericButton buttonCancel;
//  public TweenScale tweenScaleConfirmation;

//  #endregion

//  private System.Action OnConfirm { get; set; }
//  private System.Action OnCancel { get; set; }

//  public void Show(System.Action confirmDelegate, System.Action cancelDelegate, string textHeader, string textBody, string optionOk = null, string optionCancel = null)
//  {
//	this.gameObject.SetActive(true);
//	OnConfirm = confirmDelegate;
//	OnCancel = cancelDelegate;

//	labelHeader.text = textHeader;
//	labelBody.text = textBody;

//	if (string.IsNullOrEmpty(optionOk))
//	{
//	  buttonOk.ButtonText = Loca.Get("Button.Ok");
//	}
//	else
//	{
//	  buttonOk.ButtonText = optionOk;
//	}

//	if (string.IsNullOrEmpty(optionCancel))
//	{
//	  buttonCancel.ButtonText = Loca.Get("Button.Cancel");
//	}
//	else
//	{
//	  buttonCancel.ButtonText = optionCancel;
//	}

//	// colors
//	ColorManager colorManager = GameController.Instance.ColorManager;
//	buttonCancel.BackgroundColor = colorManager.ColorButtonNo;
//	buttonOk.BackgroundColor = colorManager.ColorButtonDefault;

//	tweenScaleConfirmation.ResetToBeginning();
//	tweenScaleConfirmation.PlayForward();
//  }

//  public void OnButtonConfirm()
//  {
//	DestroyPanel();
//	if (OnConfirm != null)
//	{
//	  OnConfirm();
//	}
//  }

//  public void OnButtonCancel()
//  {
//	DestroyPanel();
//	if (OnCancel != null)
//	{
//	  OnCancel();
//	}
//  }

//  public void DestroyPanel()
//  {
//	GameObject.Destroy(gameObject);
//  }
//}

