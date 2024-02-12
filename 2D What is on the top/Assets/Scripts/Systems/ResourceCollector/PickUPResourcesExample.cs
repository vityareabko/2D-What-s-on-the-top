using System;
using System.Collections.Generic;
using UnityEngine;

namespace ResourceCollector
{
    public class PickUPResourcesExample : MonoBehaviour
    {
        private ResourceCollector _resourceCollector;

        [SerializeField] private List<Coin> _listCoins;

        private void Awake()
        {
            _resourceCollector = new ResourceCollector();
        }

        private void OnEnable()
        {
            _resourceCollector.ResourcesContainerChange += OnResourcesContainerChanged;
            SubscirbeCoins();
        }

        private void OnDisable()
        {
            _resourceCollector.ResourcesContainerChange -= OnResourcesContainerChanged;
            UnsubscribeCoins();
        }

        private void SubscirbeCoins()
        {
            foreach (var coin in _listCoins)
                coin.PickUP += OnPickUPCoin;
        }

        private void UnsubscribeCoins()
        {
            foreach (var coin in _listCoins)
                coin.PickUP -= OnPickUPCoin;
        } 
        
        private void OnPickUPCoin(IPickUp coin)
        {
            _resourceCollector.AddResource(coin);
        }
        

        private void OnResourcesContainerChanged(Dictionary<ResourceTypes, int> resourcesContainer) // это должна быть вьюха
        {
            foreach (var (key, value) in resourcesContainer)
            {
                Debug.Log(resourcesContainer[key]);
            }
        }
    }
}