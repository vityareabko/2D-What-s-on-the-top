using System;
using UnityEngine;

namespace ResourcesCollector
{
    public class Coin : MonoBehaviour, IPickUp
    {
        public ResourceTypes Type { get; } = ResourceTypes.Coin;

        public event Action<IPickUp> PickUP;

        [SerializeField] private int CoinsValue = 1;

        private void Hide() => gameObject.SetActive(false);

        public int GetCoinsValue() => CoinsValue;

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