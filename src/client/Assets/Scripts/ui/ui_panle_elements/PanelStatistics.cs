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

using GameStates;

namespace UI
{
    public class PanelStatistics : AbstractPanelDeclarations, IDefaultMenuButtons
    {

        private GameController GameController { get; set; }
        private GameStateStatistics GameStateStatistics { get; set; }

        public bool HasButtonsNextPrevious => throw new NotImplementedException();

        public void OnButtonBack(Action defaultImpl)
        {
            throw new NotImplementedException();
        }

        public void OnButtonClose()
        {
            throw new NotImplementedException();
        }

        public void OnButtonNext()
        {
            throw new NotImplementedException();
        }

        public void OnButtonPrevious()
        {
            throw new NotImplementedException();
        }

        public void OnButtonCategory(WidgetButtonStatisticsCategory buttonStatisticsCategory)
        {
            SwitchToStatisticsCategory(buttonStatisticsCategory.statisticsCategory);
        }

        private void SwitchToStatisticsCategory(GameStateStatistics.GameStatisticsCategories statisticsCategory)
        {
            ChangeState("GameStateStatisticsCategory", statisticsCategory.ToString());
        }
    }
}
