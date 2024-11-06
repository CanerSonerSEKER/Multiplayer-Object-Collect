using Events;
using Photon.Pun;
using Utils;

namespace UI.MainMenu.Buttons
{
    public class LeaveRoomBTN : UIBTN
    {
        protected override void OnClick()
        {
            MainMenuEvents.LeaveRoomBTN?.Invoke();
            PhotonNetwork.LeaveRoom();
        }
    }
}