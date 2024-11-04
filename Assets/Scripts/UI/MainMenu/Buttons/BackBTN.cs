using Events;
using Utils;

namespace UI.MainMenu.Buttons
{
    public class BackBTN : UIBTN
    {
        protected override void OnClick()
        {
            MainMenuEvents.BackBTN?.Invoke();
        }
    }
}