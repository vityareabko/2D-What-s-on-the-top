using System;
using ResourcesCollector;
using Services.StorageService;
using Services.StorageService.JsonDatas;
using UnityEngine;

namespace Score
{
    public class ScoreController : IDisposable
    {
        private ResourcesJsonData _resourcesData;
        
        private IResourceCollector _resourceCollector;
        private IStorageService _storage;
        
        private ScoreController(IResourceCollector resourceCollector, IStorageService _storageService)
        {
            _resourceCollector = resourceCollector;
            _storage = _storageService;
            
            LoadResourcesData();
            
            EventAggregator.Subscribe<ResourcePickedUpEvent>(OnPickedUpHandler);
            EventAggregator.Subscribe<PlayerWinEventHandler>(OnLevelWin);
            EventAggregator.Subscribe<PlayerLoseEventHandler>(OnLevelLose);

        }

        public void Dispose()
        {
            EventAggregator.Unsubscribe<ResourcePickedUpEvent>(OnPickedUpHandler);
            EventAggregator.Unsubscribe<PlayerWinEventHandler>(OnLevelWin);
            EventAggregator.Unsubscribe<PlayerLoseEventHandler>(OnLevelLose);
        }
        
        private void OnPickedUpHandler(object sender, ResourcePickedUpEvent eventData)
        {
            _resourceCollector.AddResource(eventData.Resource);

            if (_resourcesData.Resources.ContainsKey(eventData.Resource.StorageType))
                _resourcesData.Resources[eventData.Resource.StorageType] += eventData.Resource.AmountResources;
            else 
                _resourcesData.Resources[eventData.Resource.StorageType] = eventData.Resource.AmountResources;
        }
        
        private void LoadResourcesData()
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

        private void OnLevelWin(object arg1, PlayerWinEventHandler arg2) => SaveUpdatedResourceData();

        private void OnLevelLose(object arg1, PlayerLoseEventHandler arg2) => SaveUpdatedResourceData();
        
    }
}