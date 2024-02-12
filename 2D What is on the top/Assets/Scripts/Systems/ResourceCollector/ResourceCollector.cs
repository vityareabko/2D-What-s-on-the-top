using System;
using System.Collections.Generic;

namespace Systems
{
    public class ResourceCollector : IResourceCollector
    {
        public event Action<Dictionary<ResourceType, int>> ResourcesContainerChange;

        public Dictionary<ResourceType, int> ResourcesContainer { get; } = new();


        public void AddResource(IPickable ressource)
        {
            if (ResourcesContainer.ContainsKey(ressource.Type) == false)
                ResourcesContainer[ressource.Type] = ressource.GetValue();
            else
                ResourcesContainer[ressource.Type] += ressource.GetValue();

            ResourcesContainerChange?.Invoke(ResourcesContainer);
        }

        public void Remove(IPickable ressource)
        {
            if (ResourcesContainer.ContainsKey(ressource.Type) == false)
                ResourcesContainer[ressource.Type] = ressource.GetValue();
            else
                ResourcesContainer[ressource.Type] -= ressource.GetValue();

            ResourcesContainerChange?.Invoke(ResourcesContainer);
        }
    }
}