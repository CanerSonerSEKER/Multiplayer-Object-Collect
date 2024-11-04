using System;
using Events;
using UnityEngine;
using Utils;

namespace UI.MainMenu
{
    public class MenuManager : EventListenerMono
    {
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _findRoomPanel;
        [SerializeField] private GameObject _createRoomPanel;
        [SerializeField] private GameObject _inRoomPanel;
         
        private void Start()
        {
            SetPanelActive(_mainMenuPanel);
        }

        private void SetPanelActive(GameObject panel)
        {
            _mainMenuPanel.SetActive(_mainMenuPanel == panel);
            _findRoomPanel.SetActive(_findRoomPanel == panel);
            _createRoomPanel.SetActive(_createRoomPanel == panel);
            _inRoomPanel.SetActive(_inRoomPanel == panel);
        }

        protected override void RegisterEvents()
        {
            MainMenuEvents.FindRoomBTN += OnFindRoomBTN;
            MainMenuEvents.CreateRoomBTN += OnCreateRoomBTN;
            MainMenuEvents.BackBTN += OnBackBTN;
            MainMenuEvents.CreatingRoomBTN += OnCreatingRoomBTN;
            MainMenuEvents.LeaveRoomBTN += OnLeaveRoomBTN;
        }

        private void OnLeaveRoomBTN()
        {
            SetPanelActive(_mainMenuPanel);
        }

        private void OnFindRoomBTN()
        {
            SetPanelActive(_findRoomPanel);
        }
        
        private void OnCreateRoomBTN()
        {
            SetPanelActive(_createRoomPanel);
        }
        
        private void OnBackBTN()
        {
            SetPanelActive(_mainMenuPanel);
        }

        private void OnCreatingRoomBTN()
        {
            SetPanelActive(_inRoomPanel);
        }
        
        protected override void UnRegisterEvents()
        {
            MainMenuEvents.FindRoomBTN -= OnFindRoomBTN;
            MainMenuEvents.CreateRoomBTN -= OnCreateRoomBTN;
            MainMenuEvents.BackBTN -= OnBackBTN;
            MainMenuEvents.CreatingRoomBTN -= OnCreatingRoomBTN;
            MainMenuEvents.LeaveRoomBTN -= OnLeaveRoomBTN;
        }
            
    }
}
