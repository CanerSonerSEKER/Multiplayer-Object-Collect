using System;
using Events;
using Photon.Pun;
using UnityEngine;
using Utils;
using WorldObjects;

namespace Game
{
    public class CollisionDetector : EventListenerMono
    {
        //[SerializeField] private GameObject _playerController;
        [SerializeField] private GameObject _boxOnBack;
        private bool _isPicked = true;

        //private PhotonView _photonView;


        /*private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }*/

        private void OnWoodCollision(Wood colWood)
        {
            Debug.LogWarning($"Colwood : {colWood}");
            //_photonView.RPC("BackOnBox", RpcTarget.MasterClient);
            _boxOnBack.SetActive(true);
        }

        
        private void OnTriggerEnter(Collider player)
        {
            if (player.TryGetComponent(out Wood colWood) && _isPicked)
            {
                GameEvents.WoodCollision?.Invoke(colWood);
                colWood.OnPickable();
                _isPicked = false;
            }
            else
            {
                Debug.LogWarning($"You can't pick another box!! ");
            }
        }

        /*[PunRPC]
        private void BackOnBox()
        {
            _boxOnBack.SetActive(true);
        }*/

        protected override void RegisterEvents()
        {
            GameEvents.WoodCollision += OnWoodCollision;
        }

        protected override void UnRegisterEvents()
        {
            GameEvents.WoodCollision -= OnWoodCollision;
        }
    }
}