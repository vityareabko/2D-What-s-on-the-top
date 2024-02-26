using System.Collections.Generic;
using UnityEngine;

namespace Systems.ResourcesLoaderSystem
{
    public class ResourceLoaderSystem : IResourceLoaderSystem
    {
        private Dictionary<ResourceID, Object> _cache = new ();
        
        public T Load<T>(ResourceID resourceID) where T : Object
        {
            if (_cache.TryGetValue(resourceID, out Object value))
                return value as T;

            var resourcePath = ResourcePath.GetPath(resourceID);
            T resource = Resources.Load<T>(resourcePath);
            
            Debug.Log($"{resource} sholud be not null");
            if (resource == null)
            {
                Debug.LogError($"ResourceLoader: Не удалось загрузить ресурс по пути '{resourceID}'.");
                return null;
            }

            _cache[resourceID] = resource;
            return resource;
        }
        
        public void ClearCache()
        {
            _cache.Clear();
        }
    }
}