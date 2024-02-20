using System;
using System.Collections.Generic;

namespace ResourcesCollector
{
    public class ResourceCollector : IResourceCollector
    {
        public event Action<Dictionary<ResourceStorageTypes, int>> ResourcesContainerChange;

        public Dictionary<ResourceStorageTypes, int> ResourcesContainer { get; } = new();
        
        public void AddResource(IPickUp ressource)
        {
            if (ResourcesContainer.ContainsKey(ressource.StorageType) == false)
                ResourcesContainer[ressource.StorageType] = ressource.AmountResources;
            else
                ResourcesContainer[ressource.StorageType] += ressource.AmountResources;

            ResourcesContainerChange?.Invoke(ResourcesContainer);
        }

        public void Remove(IPickUp ressource)
        {
            if (ResourcesContainer.ContainsKey(ressource.StorageType) == false)
                ResourcesContainer[ressource.StorageType] = ressource.AmountResources;
            else
                ResourcesContainer[ressource.StorageType] -= ressource.AmountResources;

            ResourcesContainerChange?.Invoke(ResourcesContainer);
        }
    }
}