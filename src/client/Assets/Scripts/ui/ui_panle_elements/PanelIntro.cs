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
using Valve.VR;

#endregion

public class PanelIntro : AbstractPanelDeclarations
{

    public GameObject buttonSkip;

    private GameController GameController { get; set; }

    public UILabel labelVersionInfo;

    protected override void OnInitialize()
    {
        GameController = GameController.Instance;
    }

    protected override void OnEnter(object onEnterParams)
    {
        NGUITools.SetActive(buttonSkip, false);
        string version = "Version: " + GameGlobals.FullAppVersion;
        labelVersionInfo.text = version;
    }

    protected override void OnUpdate()
    {
    }

    public void ActivateSkipButton()
    {
        NGUITools.SetActive(buttonSkip, true);
    }

    public void OnButtonSkip()
    {
        FsmSystem.ChangeState("GameStateReadyForLogin");
    }

}