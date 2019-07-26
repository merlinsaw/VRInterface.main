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
    public class WidgetStatisticsOverview : MonoBehaviour
    {

        //public UITable tableCategoryButtons;

        private PanelStatistics panelStatistics;

        public WidgetButtonStatisticsCategory satisticsButtonSummary;
        public WidgetButtonStatisticsCategory satisticsButtonMatchHistory;

        private Dictionary<GameStateStatistics.GameStatisticsCategories, WidgetButtonStatisticsCategory> StatisticsButtons { get; set; }

        private void InitializeStatisticsButtons()
        {
            StatisticsButtons = new Dictionary<GameStateStatistics.GameStatisticsCategories, WidgetButtonStatisticsCategory>();
            StatisticsButtons.Add(GameStateStatistics.GameStatisticsCategories.Summary, satisticsButtonSummary);
            StatisticsButtons.Add(GameStateStatistics.GameStatisticsCategories.MatchHistory, satisticsButtonMatchHistory);
        }

        public void SetUpOverview(PanelStatistics panel)
        {
            if (StatisticsButtons == null)
            {
                InitializeStatisticsButtons();
            }

            panelStatistics = panel;
            //tableCategoryButtons.gameObject.SetActive(true);
            //tableCategoryButtons.Reposition();

            foreach (WidgetButtonStatisticsCategory widgetStatisticsButton in StatisticsButtons.Values)
            {
                // any values that are important for the options overview


            }


        }

        public void ClearOverview()
        {
            //tableCategoryButtons.gameObject.SetActive(false);
        }

        public void OnButtonCategory(GameObject sender)
        {
            WidgetButtonStatisticsCategory buttonStatisticsCategory = sender.GetComponent<WidgetButtonStatisticsCategory>();
            panelStatistics.OnButtonCategory(buttonStatisticsCategory);
        }   


    }

}


