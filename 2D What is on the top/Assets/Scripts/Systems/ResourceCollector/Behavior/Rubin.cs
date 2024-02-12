using System;
using UnityEngine;

namespace ResourceCollector
{
    public class Rubin : MonoBehaviour, IPickUp
    {
        public ResourceTypes Type { get; } = ResourceTypes.Rubin;

        public event Action<IPickUp> PickUP;

        [SerializeField] private int CoinsValue = 1;
        
        public int GetValue() => CoinsValue;
        
        private void Hide() => gameObject.SetActive(false);

        public void OnTriggerEnter2D(Collider2D colider)
        {
            PickUP?.Invoke(this);
            
            Hide();

        }
    }
}