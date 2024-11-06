using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace UI.MainMenu.Buttons
{
    public class PlayerListItem : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_Text _playerNameTMP;
        private Player _player;

        public void SetUp(Player player)
        {
            _player = player;
            _playerNameTMP.text = _player.NickName;
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (_player == otherPlayer)
            {
                Destroy(gameObject);
            }
        }

        public override void OnLeftRoom()
        {
            Destroy(gameObject);
        }
    }
}