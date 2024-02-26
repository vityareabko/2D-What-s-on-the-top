using System.Collections.Generic;
using UnityEngine;

namespace Systems.ResourcesLoaderSystem
{
    public static class ResourcePath
    {
        private static readonly Dictionary<ResourceID, string> paths = new Dictionary<ResourceID, string>
        {
            { ResourceID.Level1Prefab, "Prefabs/Level1" },
            { ResourceID.TestLevelPrefab, "Prefabs/TestLevel" },
        };

        public static string GetPath(ResourceID id)
        {
            if (paths.TryGetValue(id, out string path))
            {
                return path;
            }
            else
            {
                Debug.LogError($"Path not found for resource ID: {id}");
                return null;
            }
        }
    }
}