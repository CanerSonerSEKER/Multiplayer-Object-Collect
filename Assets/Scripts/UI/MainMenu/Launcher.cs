using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UI.MainMenu.Buttons;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI.MainMenu
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        public static Launcher Instance;
        
        [SerializeField] private TMP_Text _roomNameText;
        [SerializeField] private Transform _roomListContent;
        [SerializeField] private GameObject _roomListItemPrefab;
        
        private string _gameVersion = "1";

        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;
            
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

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void JoinRoom(RoomInfo info)
        {
            PhotonNetwork.JoinRoom(info.Name);
        }
        
        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room");

            _roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Pun Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            PhotonNetwork.JoinLobby();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("Pun Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason{0}", cause);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinRandomFailed() was called by PUN. No room available, so we create one.\nCalling:  PhotonNetwork.CreateRoom");
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Lobiye girildi. ");
            PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
            
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            foreach (Transform roomsTrans in _roomListContent)
            {
                Destroy(roomsTrans.gameObject);
            }

            foreach (RoomInfo room in roomList)
            {
                if (room.RemovedFromList)
                {
                    continue;
                }

                Instantiate(_roomListItemPrefab, _roomListContent).GetComponent<RoomListItem>().SetUp(room);
            }
            
        }

    }
}
