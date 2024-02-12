using System;
using System.Collections.Generic;

namespace ResourceCollector
{
    public interface IResourceCollector
    {
        public event Action<Dictionary<ResourceTypes, int>> ResourcesContainerChange;
        public Dictionary<ResourceTypes, int> ResourcesContainer { get; }

        public void AddResource(IPickUp resource);

        public void Remove(IPickUp resource);
    }
}