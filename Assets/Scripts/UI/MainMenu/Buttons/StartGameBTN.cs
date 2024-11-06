using Events;
using Photon.Pun;
using Utils;

namespace UI.MainMenu.Buttons
{
    public class StartGameBTN : UIBTN
    {
        protected override void OnClick()
        {
            MainMenuEvents.StartGameBTN?.Invoke();
        }
    }
}