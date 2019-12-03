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
    public class GameStateStatistics : AbstractFSMState
    {

        public enum GameStatisticsCategories
        {
            Summary,
            MatchHistory,
        }


        public GameStateStatistics(string stateName, params AbstractPanelDeclarations[] panelDeclarations)
          : base(stateName, new List<AbstractPanelDeclarations>(panelDeclarations))
        {
        }

        protected override void OnInitialize()
        {
            throw new NotImplementedException();
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
