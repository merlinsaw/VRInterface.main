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

namespace GameStates
{

    public class GameStateOptions : AbstractFSMState
    {

        public enum GameOptionsCategories
        {
            Overview,
            Grafik,
            Sound,
            Controls
        }

        public GameStateOptions(string stateName, params AbstractPanelDeclarations[] panelDeclarations)
          : base(stateName, new List<AbstractPanelDeclarations>(panelDeclarations))
        {
        }

        protected override void OnInitialize()
        {
        }

        protected override void OnEnter(object onEnterParams)
        {
        }

        protected override void OnLeave()
        {
        }

        public void ChangeStateToApplySetup(string itemId)
        {
            ChangeState("GameStateApplySetup", itemId);
        }
    }

}