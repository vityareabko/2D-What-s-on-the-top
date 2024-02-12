using System;
using System.Collections.Generic;

namespace Systems
{
    public interface IResourceCollector
    {
        public event Action<Dictionary<ResourceType, int>> ResourcesContainerChange;
        public Dictionary<ResourceType, int> ResourcesContainer { get; }

        public void AddResource(IPickable resource);

        public void Remove(IPickable resource);
    }
}