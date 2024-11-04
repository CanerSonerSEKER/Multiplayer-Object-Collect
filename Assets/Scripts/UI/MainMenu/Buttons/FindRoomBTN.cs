using Events;
using Utils;

namespace UI.MainMenu.Buttons
{
    public class FindRoomBTN : UIBTN
    {
        protected override void OnClick()
        {
            MainMenuEvents.FindRoomBTN?.Invoke();
        }
    }
}