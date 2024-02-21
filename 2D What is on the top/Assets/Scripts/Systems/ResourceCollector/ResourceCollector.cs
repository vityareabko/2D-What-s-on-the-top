using System;
using System.Collections.Generic;

namespace ResourcesCollector
{
    public class ResourceCollector : IResourceCollector
    {
        public event Action<Dictionary<ResourceTypes, int>> ResourcesContainerChange;

        public Dictionary<ResourceTypes, int> ResourcesContainer { get; } = new();
        
        public void AddResource(IPickUp ressource)
        {
            if (ResourcesContainer.ContainsKey(ressource.Type) == false)
                ResourcesContainer[ressource.Type] = ressource.AmountResources;
            else
                ResourcesContainer[ressource.Type] += ressource.AmountResources;

            ResourcesContainerChange?.Invoke(ResourcesContainer);
        }

        public void Remove(IPickUp ressource)
        {
            if (ResourcesContainer.ContainsKey(ressource.Type) == false)
                ResourcesContainer[ressource.Type] = ressource.AmountResources;
            else
                ResourcesContainer[ressource.Type] -= ressource.AmountResources;

            ResourcesContainerChange?.Invoke(ResourcesContainer);
        }
    }
}