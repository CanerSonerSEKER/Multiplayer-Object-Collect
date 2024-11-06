using Photon.Pun;
using UnityEngine;
using Utils;

namespace Game.PlayersScripts
{
    public class PlayerManager : EventListenerMono
    {
        [SerializeField] private PhotonView _photonView;

        /*
        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }
        */

        private void Start()
        {
            if (_photonView.IsMine)
            {
                CreateController();
            }            
        }

        private void CreateController()
        {
            // PhotonNetwork.Instantiate()
        }


        protected override void RegisterEvents()
        {
            
        }

        protected override void UnRegisterEvents()
        {
        }
    }
}