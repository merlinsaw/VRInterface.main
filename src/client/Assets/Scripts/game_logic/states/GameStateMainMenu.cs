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
    public class GameStateMainMenu : AbstractFSMState
    {
        public enum MainMenuCategories
        {
            Overview,
            Options,
            Statistics,
        }
        public GameStateMainMenu(string stateName, params AbstractPanelDeclarations[] panelDeclarations)
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

        public void ChangeStateToStatistics(string itemId)
        {
            ChangeState("GameStateStatistics", itemId);
        }

    }


   
}
