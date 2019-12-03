//-------------------------------------------------------
//	
//-------------------------------------------------------

#region using

using UnityEngine;

#endregion

public sealed class ColorManager : MonoBehaviour
{

  //Important: alpha-value has to be set to 1 (default = 0) !!
  public Color colorButtonDefault;
  public Color ColorButtonDefault
  {
	get
	{
	  colorButtonDefault.a = 1;
	  return colorButtonDefault;
	}
	private set
	{
	  colorButtonDefault = value;
	}
  }

  public Color colorButtonNo = Color.red;
  public Color ColorButtonNo
  {
	get
	{
	  colorButtonNo.a = 1;
	  return colorButtonNo;
	}
	private set
	{
	  colorButtonNo = value;
	}
  }

  public Color colorGreenMark;
  public Color ColorGreenMark
  {
	get
	{
	  colorGreenMark.a = 1;
	  return colorGreenMark;
	}
	private set
	{
	  colorGreenMark = value;
	}
  }

  public Color defaultInfoTextColor = Color.white;
  public Color DefaultInfoTextColor
  {
	get
	{
	  defaultInfoTextColor.a = 1;
	  return defaultInfoTextColor;
	}
	private set
	{
	  defaultInfoTextColor = value;
	}
  }

  public Color defaultErrorTextColor = Color.red;
  public Color DefaultErrorTextColor
  {
	get
	{
	  return defaultErrorTextColor;
	}
	private set
	{
	  defaultErrorTextColor = value;
	}
  }
}
