using System;
using PersistentData;
using ResourcesCollector;
using Services.StorageService.JsonDatas;

namespace Score
{
    public class ScoreController : IDisposable
    {
        private ResourcesJsonData _resourcesData;
        
        private IResourceCollector _resourceCollector;
        private IPersistentResourceData _walletResource;
        
        private ScoreController(IResourceCollector resourceCollector, IPersistentResourceData walletResource)
        {
            _resourceCollector = resourceCollector;
            _walletResource = walletResource;
            EventAggregator.Subscribe<ResourcePickedUpEvent>(OnPickedUpHandler);
        }

        public void Dispose() => EventAggregator.Unsubscribe<ResourcePickedUpEvent>(OnPickedUpHandler);
        
        private void OnPickedUpHandler(object sender, ResourcePickedUpEvent eventData)
        {
            _resourceCollector.AddResource(eventData.Resource);
            _walletResource.ResourcesJsonData.Add(eventData.Resource.Type, eventData.Resource.AmountResources);
            _walletResource.SaveData();
        }
    }
}
