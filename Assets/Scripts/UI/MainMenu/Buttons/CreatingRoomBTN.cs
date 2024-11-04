using Events;
using Utils;

namespace UI.MainMenu.Buttons
{
    public class CreatingRoomBTN : UIBTN
    {
        protected override void OnClick()
        {
            MainMenuEvents.CreatingRoomBTN?.Invoke();
        }
    }
}