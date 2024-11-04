using Events;
using Utils;

namespace UI.MainMenu.Buttons
{
    public class CreateRoomBTN : UIBTN
    {
        protected override void OnClick()
        {
            MainMenuEvents.CreateRoomBTN?.Invoke();
        }
    }
}