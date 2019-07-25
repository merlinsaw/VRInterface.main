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

namespace Assets.Scripts.game_logic.states
{
    public class GameStateStartupGame : AbstractFSMState
    {

        private bool IsGameDataLoaded { get; set; }
        private bool HasIntroFinished { get; set; }

        private PanelIntro PanelIntro { get; set; }


        public GameStateStartupGame(string stateName, AbstractPanelDeclarations panelIntro)
          : base(stateName, panelIntro)
        {
            IsGameDataLoaded = false;
            HasIntroFinished = true; //TODO: just until intro is implemented, then set correctly
        }

        protected override void OnInitialize()
        {
            PanelIntro = GetPanel<PanelIntro>();
            IsGameDataLoaded = false;
        }

    }
}
