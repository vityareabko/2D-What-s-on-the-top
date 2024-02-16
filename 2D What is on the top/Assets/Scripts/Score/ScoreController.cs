using System.Collections.Generic;
using Game.Gameplay;
using ResourcesCollector;
using Services.StorageService;
using Services.StorageService.JsonDatas;
using UnityEngine;
using Zenject;


namespace Score
{
    public class ScoreController : MonoBehaviour
    {
        [SerializeField] private List<Coin> _listLevelCoins;
        
        private ResourcesJsonData _resourcesData;
        
        private IResourceCollector _resourceCollector;
        private IStorageService _storage;
        
        [Inject] private void Construct(IResourceCollector resourceCollector, IStorageService _storageService)
        {
            _resourceCollector = resourceCollector;
            _storage = _storageService;
        }

        private void Awake()
        {
            LoadData();
        }

        private void OnEnable()
        {
            SubscriberCoins();
            
            EventAggregator.Subscribe<PlayerWinEventHandler>(OnLevelWin);
            EventAggregator.Subscribe<PlayerLoseEventHandler>(OnLevelLose);
        }

        private void OnDisable()
        {
            UnsubscriberCoins();
            
            EventAggregator.Unsubscribe<PlayerWinEventHandler>(OnLevelWin);
            EventAggregator.Unsubscribe<PlayerLoseEventHandler>(OnLevelLose);
        }

        private void SubscriberCoins()
        {
            foreach (var coin in _listLevelCoins)
                coin.PickUP += OnPickUPCoin;
        }

        private void UnsubscriberCoins()
        {
            foreach (var coin in _listLevelCoins)
                coin.PickUP -= OnPickUPCoin;
        }

        private void LoadData()
        {
            _storage.Load<ResourcesJsonData>(StorageKeysType.Resources, data =>
            {
                if (data != null)
                    _resourcesData = data;
                else
                    _resourcesData = new ResourcesJsonData();
            });
        }

        private void SaveUpdatedResourceData()
        {
            _storage.Save(StorageKeysType.Resources, _resourcesData, b =>
            {
                if (b)
                    Debug.Log("success save");
                else
                    Debug.Log("Failed save");
            });
        }

        private void OnPickUPCoin(IPickUp coin)
        {
            _resourceCollector.AddResource(coin);
            
            if (_resourcesData.Coins.ContainsKey(coin.Type))
                _resourcesData.Coins[coin.Type] += coin.GetCoinsValue();
            else 
                _resourcesData.Coins[coin.Type] = coin.GetCoinsValue();
        }

        private void OnLevelWin(object arg1, PlayerWinEventHandler arg2) => SaveUpdatedResourceData();

        private void OnLevelLose(object arg1, PlayerLoseEventHandler arg2) => SaveUpdatedResourceData();
        
    }
}