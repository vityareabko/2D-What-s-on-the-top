using System;
using System.Collections.Generic;
using Services.StorageService;
using Services.StorageService.JsonDatas;
using UnityEngine;
using Zenject;

namespace ResourcesCollector
{
    public class PickUPResourcesExample : MonoBehaviour
    {
        private ResourceCollector _resourceCollector;

        [SerializeField] private List<Coin> _listCoins;

        private IStorageService _storage;
        
        [Inject] public void Constructor(IStorageService storage)
        {
            _storage = storage;
        }

        private void Awake()
        {
            _resourceCollector = new ResourceCollector();
        }

        private void OnEnable()
        {
            // _resourceCollector.ResourcesContainerChange += OnResourcesContainerChanged;
            SubscirbeCoins();
        }

        private void OnDisable()
        {
            // _resourceCollector.ResourcesContainerChange -= OnResourcesContainerChanged;
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

            var resourcesData = new ResourcesJsonData();
            _storage.Load<ResourcesJsonData>(StorageKeysType.Resources, data =>
            {
                if (data != null)
                    resourcesData = data;
            });
            
            if (resourcesData.Coins.ContainsKey(coin.Type))
                resourcesData.Coins[coin.Type] += coin.GetCoinsValue();
            else 
                resourcesData.Coins[coin.Type] = coin.GetCoinsValue();
            
            _storage.Save(StorageKeysType.Resources, resourcesData, b =>
            {
                if (b)
                    Debug.Log("success save");
                else
                    Debug.Log("Failed save");
            });
            // проверить сохранения монет
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