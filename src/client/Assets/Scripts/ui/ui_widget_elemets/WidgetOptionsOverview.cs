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

namespace UI { 

public class WidgetOptionsOverview : MonoBehaviour
    {

        //public UITable tableCategoryButtons;

        private PanelMainMenu panelMainMenu;

        public WidgetButtonMainMenuCategory mainMenuButtonGrafik;
        public WidgetButtonMainMenuCategory mainMenuButtonStatistics;

        private Dictionary<GameStateMainMenu.MainMenuCategories, WidgetButtonMainMenuCategory> MainMenuButtons { get; set; }

        private void InitializeMainMenuButtons()
        {
            MainMenuButtons = new Dictionary<GameStateMainMenu.MainMenuCategories, WidgetButtonMainMenuCategory>();
            MainMenuButtons.Add(GameStateMainMenu.MainMenuCategories.Options, mainMenuButtonGrafik);
            MainMenuButtons.Add(GameStateMainMenu.MainMenuCategories.Statistics, mainMenuButtonStatistics);
        }

        public void SetUpOverview(PanelMainMenu panel)
        {
            if (MainMenuButtons == null)
            {
                InitializeMainMenuButtons();
            }

            panelMainMenu = panel;
            //tableCategoryButtons.gameObject.SetActive(true);
            //tableCategoryButtons.Reposition();

            foreach (WidgetButtonMainMenuCategory widgetMainMenuButton in MainMenuButtons.Values)
            {
                // any values that are important for the options overview

                //int upgradesCount = GameController.Instance.UpgradeRecommendationService.GetAvailableShopUpgradesCount(widgetMainMenuButton.mainMenuCategory);
                //widgetMainMenuButton.SetNewItemsCount(upgradesCount);
            }

            
        }

        public void ClearOverview()
        {
            //tableCategoryButtons.gameObject.SetActive(false);
        }

        public void OnButtonCategory(GameObject sender)
        {
            WidgetButtonMainMenuCategory buttonMainMenuCategory = sender.GetComponent<WidgetButtonMainMenuCategory>();
            panelMainMenu.OnButtonCategory(buttonMainMenuCategory);
        }


    }

}
