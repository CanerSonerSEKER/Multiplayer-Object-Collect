using UnityEngine;
using UnityEngine.Events;

namespace WorldObjects
{
    public class Wood : MonoBehaviour
    {
        public event UnityAction<Wood> Pickable;

        public void OnPickable()
        {
            Pickable?.Invoke(this);
        }
    }
}