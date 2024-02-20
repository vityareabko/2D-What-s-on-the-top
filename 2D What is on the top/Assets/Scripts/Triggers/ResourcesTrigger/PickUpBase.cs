using System;
using UnityEngine;

namespace ResourcesCollector
{
    public abstract class PickUpBase : MonoBehaviour, IPickUp
    {
        public abstract ResourceStorageTypes StorageType { get; }

        // public event Action<IPickUp> PickUP;

        [field: SerializeField] public int AmountResources { get; private set; } = 1; // пока что через инспектор задаем кол-во рессурса

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