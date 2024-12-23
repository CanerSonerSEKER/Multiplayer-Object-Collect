using Events;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Utils;
using WorldObjects;

namespace Game
{
    public class CollisionDetector : EventListenerMono
    {
        [SerializeField] private GameObject _boxOnBack;
        private bool _isPicked = true;

        private void OnWoodCollision(Wood colWood)
        {
            Debug.LogWarning($"Colwood : {colWood}");
            _boxOnBack.SetActive(true);
        }
        
        private void OnBaseCollision(Base colBase)
        {
            Debug.LogWarning($"Base Trigger");
            _boxOnBack.SetActive(false);
            
        }

        private void OnTriggerEnter(Collider player)
        {

            if (player.TryGetComponent(out Base colBase) && _isPicked)
            {
                GameEvents.BaseCollision?.Invoke(colBase);
                colBase.OnGiven();
                _isPicked = false;
            }
            
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
            GameEvents.BaseCollision += OnBaseCollision;
        }

        protected override void UnRegisterEvents()
        {
            GameEvents.WoodCollision -= OnWoodCollision;
            GameEvents.BaseCollision -= OnBaseCollision;
        }
    }
}