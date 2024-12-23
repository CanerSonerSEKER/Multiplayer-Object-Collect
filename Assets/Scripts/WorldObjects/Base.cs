using UnityEngine;
using UnityEngine.Events;

namespace WorldObjects
{
    public class Base : MonoBehaviour
    {
        public event UnityAction<Base> Given;

        public void OnGiven()
        {
            Given?.Invoke(this);
        }
    }
}