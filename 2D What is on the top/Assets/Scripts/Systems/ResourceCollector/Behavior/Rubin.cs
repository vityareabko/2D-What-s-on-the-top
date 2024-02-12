using System;
using UnityEngine;

namespace ResourceCollector
{
    public class Rubin : MonoBehaviour, IPickable
    {
        public ResourceType Type { get; } = ResourceType.Rubin;

        public event Action<ResourceType> PickUP;

        [SerializeField] private int CoinsValue = 1;

        public int GetValue() => CoinsValue;

        public void OnTriggerEnter(Collider colider) => PickUP?.Invoke(Type);
    }
}