using System;
using UnityEngine;

namespace ResourcesCollector
{
    public class PickUpBase : MonoBehaviour, IPickUp
    {
        public ResourceTypes Type { get; }

        // public event Action<IPickUp> PickUP;

        [field: SerializeField] public int AmountResources { get; private set; } = 1; 

        private void Hide() => gameObject.SetActive(false);
        
        public void OnTriggerEnter2D(Collider2D colider)
        {
            if (colider.CompareTag(ConstTags.Player))
            {
                // PickUP?.Invoke(this);
                EventAggregator.Post(this, new ResourcePickedUpEvent(this));
                Hide();
            }
        }
    }
}