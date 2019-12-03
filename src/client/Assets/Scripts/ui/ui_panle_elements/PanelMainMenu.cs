﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using GameStates;

namespace UI
{ //: MonoBehaviour {//
    public class PanelMainMenu : AbstractPanelDeclarations, IDefaultMenuButtons
    {


        private GameController GameController { get; set; }
        private GameStateOptions GameStateOptions { get; set; }

        protected override void OnInitialize()
        {
            GameController = GameController.Instance;
        }

        protected override void OnEnter(object onEnterParams)
        {
            //prepare panel for the current GameState
            GameStateOptions = State as GameStateOptions;
            //widgetOptionsOverview.SetUpOverview(this);
        }



        public bool IsOpen { get; set; }



        public void Open()
        {
            if (!IsOpen)
            {
                IsOpen = true;
            }
            else
            {
                Debug.Log("Menu is already open");
            }
        }

        #region IDefaultMenuButtons
        public void OnButtonClose()
        {
            //FsmSystem.ChangeState("GameStateMainView");
        }

        public void OnButtonBack(Action defaultImpl)
        {
            defaultImpl.Invoke();
        }

        public void OnButtonNext()
        {
        }

        public void OnButtonPrevious()
        {
        }

        public bool HasButtonsNextPrevious
        {
            get { return (false); }
        }
        #endregion

        public void OnAdvancedButton()
        {
            Debug.Log("MainMenu.AdvancedButton: Do something");
        }

        public void OnButtonCategory(WidgetButtonMainMenuCategory buttonMainMenuCategory)
        {
            SwitchToMainMenuCategory(buttonMainMenuCategory.mainMenuCategory);
        }

        private void SwitchToMainMenuCategory(GameStateMainMenu.MainMenuCategories mainMenuCategory)
        {
            ChangeState("GameStateMainMenuCategory", mainMenuCategory.ToString());
        }
    }
}