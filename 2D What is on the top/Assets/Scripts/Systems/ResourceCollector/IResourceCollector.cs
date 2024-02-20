using System;
using System.Collections.Generic;

namespace ResourcesCollector
{
    public interface IResourceCollector
    {
        public event Action<Dictionary<ResourceStorageTypes, int>> ResourcesContainerChange;
        
        public Dictionary<ResourceStorageTypes, int> ResourcesContainer { get; }

        public void AddResource(IPickUp resource);

        public void Remove(IPickUp resource);
    }
}