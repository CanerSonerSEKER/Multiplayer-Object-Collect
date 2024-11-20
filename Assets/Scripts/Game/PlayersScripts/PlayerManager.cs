using System.IO;
using Photon.Pun;
using UnityEngine;
using Utils;

namespace Game.PlayersScripts
{
    public class PlayerManager : EventListenerMono
    {
        [SerializeField] private PhotonView _photonView;

        private void Start()
        {
            if (_photonView.IsMine)
            {
                CreateController();
            }            
        }

        private void CreateController()
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerRobot"), Vector3.up * 1.1f, Quaternion.identity);
        }


        protected override void RegisterEvents()
        {
            
        }

        protected override void UnRegisterEvents()
        {
        }
    }
}