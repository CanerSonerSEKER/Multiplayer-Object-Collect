using Events;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Utils;

namespace UI.MainMenu
{
    public class RoomListItem : UIBTN
    {
        [SerializeField] private TMP_Text _text;
        private RoomInfo _info;

        public void SetUp(RoomInfo info)
        {
            _info = info;
            _text.text = _info.Name;
        }

        protected override void OnClick()
        {
            Launcher.Instance.JoinRoom(_info);
            MainMenuEvents.RoomListItemBTN?.Invoke();
        }
    } 
}