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
            EventAggregator.Subscribe<SwitchGameStateToLoseGameEvent>(OnLevelLose);
            // EventAggregator.Subscribe<PlayerLoseEventHandler>(OnLevelLose);

        }

        public void Dispose()
        {
            EventAggregator.Unsubscribe<ResourcePickedUpEvent>(OnPickedUpHandler);
            EventAggregator.Unsubscribe<SwitchGameStateToLoseGameEvent>(OnLevelLose);
            // EventAggregator.Unsubscribe<PlayerLoseEventHandler>(OnLevelLose);
        }
        
        private void OnPickedUpHandler(object sender, ResourcePickedUpEvent eventData)
        {
            _resourceCollector.AddResource(eventData.Resource);

            if (_resourcesData.Resources.ContainsKey(eventData.Resource.Type))
                _resourcesData.Resources[eventData.Resource.Type] += eventData.Resource.AmountResources;
            else 
                _resourcesData.Resources[eventData.Resource.Type] = eventData.Resource.AmountResources;
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

        private void OnLevelLose(object sender, SwitchGameStateToLoseGameEvent eventData) => SaveUpdatedResourceData();
        
    }
}