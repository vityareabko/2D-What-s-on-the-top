using System;
using UnityEngine;

namespace ResourcesCollector
{
    public class Rubin : MonoBehaviour, IPickUp
    {
        public ResourceTypes Type { get; } = ResourceTypes.Rubin;

        public event Action<IPickUp> PickUP;

        [SerializeField] private int CoinsValue = 1;
        
        public int GetCoinsValue() => CoinsValue;
        
        private void Hide() => gameObject.SetActive(false);

        public void OnTriggerEnter2D(Collider2D colider)
        {
            if (colider.CompareTag(ConstTags.Player))
            {
                PickUP?.Invoke(this);
                Hide();
            }
        }
    }
}