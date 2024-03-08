using System;
using WalletResources;
using ResourcesCollector;
using Services.StorageService.JsonDatas;

namespace Score
{
    public class ScoreController : IDisposable
    {
        private ResourcesJsonData _resourcesData;
        
        private IResourceCollector _resourceCollector;
        private IWalletResource _walletResource;
        
        private ScoreController(IResourceCollector resourceCollector, IWalletResource walletResource)
        {
            _resourceCollector = resourceCollector;
            _walletResource = walletResource;
            EventAggregator.Subscribe<ResourcePickedUpEvent>(OnPickedUpHandler);
        }

        public void Dispose() => EventAggregator.Unsubscribe<ResourcePickedUpEvent>(OnPickedUpHandler);
        
        private void OnPickedUpHandler(object sender, ResourcePickedUpEvent eventData)
        {
            _resourceCollector.AddResource(eventData.Resource);
            _walletResource.Add(eventData.Resource.Type, eventData.Resource.AmountResources);
        }
    }
}
