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



        public GameStateStartupGame(string stateName, AbstractFSMStateController stateController, AbstractPanelDeclarations panelDeclarations) 
            : base(stateName, stateController, panelDeclarations)
        {
        }


        public GameStateStartupGame(string stateName, List<AbstractPanelDeclarations> panelDeclarations, AbstractFSMStateController stateController) 
            : base(stateName, panelDeclarations, stateController)
        {
        }

        public GameStateStartupGame(string stateName, List<AbstractPanelDeclarations> panelDeclarations) 
            : base(stateName, panelDeclarations)
        {
        }

        protected override void OnInitialize()
        {
            PanelIntro = GetPanel<PanelIntro>();
            IsGameDataLoaded = false;
        }

        protected override void OnEnter(object onEnterParams)
        {
            throw new NotImplementedException();
        }

        protected override void OnLeave()
        {
            throw new NotImplementedException();
        }
    }
}
