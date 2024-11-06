using Events;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.MainMenu.Buttons
{
    public class CreatingRoomBTN : UIBTN
    {
        [SerializeField] private TMP_InputField _roomNameInput;
        private void CreateRoom()
        {
            Debug.LogWarning("SELAMLAR ODA AÇILDI...");
            if (string.IsNullOrEmpty(_roomNameInput.text))
            {
                return;
            }
            
            RoomOptions roomOptions = new RoomOptions {MaxPlayers = 4};

            PhotonNetwork.CreateRoom(_roomNameInput.text, roomOptions);

        }

        protected override void OnClick()
        {
            MainMenuEvents.CreatingRoomBTN?.Invoke();
            CreateRoom();
        }
        
        
    }
}