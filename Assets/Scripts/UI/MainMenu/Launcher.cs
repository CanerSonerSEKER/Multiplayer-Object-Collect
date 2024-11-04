using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace UI.MainMenu
{
    public class Launcher : MonoBehaviour
    {
        private string _gameVersion = "1";

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            Connect();
        }

        private void Connect()
        {
            
            if (PhotonNetwork.IsConnected)
            {
                //PhotonNetwork.JoinRoom();
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = _gameVersion;
            }
            
        }
    }
}
