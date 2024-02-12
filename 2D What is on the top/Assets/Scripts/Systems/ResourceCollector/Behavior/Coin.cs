using System;
using UnityEngine;

namespace ResourceCollector
{
    public class Coin : MonoBehaviour, IPickUp
    {
        public ResourceTypes Type { get; } = ResourceTypes.Coin;

        public event Action<IPickUp> PickUP;

        [SerializeField] private int CoinsValue = 1;

        private void Hide() => gameObject.SetActive(false);

        public int GetValue() => CoinsValue;

        public void OnTriggerEnter2D(Collider2D colider)
        {
            PickUP?.Invoke(this);
            Hide();
        }
    }
}