using System;
using Events;
using UnityEngine;
using Utils;
using WorldObjects;

namespace Game
{
    public class CollisionDetector : EventListenerMono
    {
        //[SerializeField] private GameObject _playerController;
        private bool _isPicked = true;
        
        
        private void OnWoodCollision(Wood colWood)
        {
            Debug.LogWarning($"Colwood : {colWood}");
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