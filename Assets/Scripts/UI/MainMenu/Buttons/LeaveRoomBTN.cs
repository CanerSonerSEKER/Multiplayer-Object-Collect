using Events;
using Utils;

namespace UI.MainMenu.Buttons
{
    public class LeaveRoomBTN : UIBTN
    {
        protected override void OnClick()
        {
            MainMenuEvents.LeaveRoomBTN?.Invoke();
        }
    }
}