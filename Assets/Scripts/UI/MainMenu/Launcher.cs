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
        [SerializeField] private TMP_Text _inRoomNameText;
        [SerializeField] private Transform _roomListContent;
        [SerializeField] private GameObject _roomListItemPrefab;
        [SerializeField] private Transform _playerListContent;
        [SerializeField] private GameObject _playerListItemPrefab;
        [SerializeField] private GameObject _startGameButton;
        
        private string _gameVersion = "1";

        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;
            
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            PhotonNetwork.GameVersion = _gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }

        public void LoadScene()
        {
            PhotonNetwork.LoadLevel(1);
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
            _inRoomNameText.text = PhotonNetwork.CurrentRoom.Name;

            Player[] players = PhotonNetwork.PlayerList;

            foreach (Transform child in _playerListContent)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < players.Length; i++)
            {
                Instantiate(_playerListItemPrefab, _playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
            }

            _startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            _startGameButton.SetActive(PhotonNetwork.IsMasterClient);
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

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Instantiate(_playerListItemPrefab, _playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
        }

    }
}
