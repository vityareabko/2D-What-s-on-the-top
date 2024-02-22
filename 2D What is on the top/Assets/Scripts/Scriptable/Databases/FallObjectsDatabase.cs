using System.Collections.Generic;
using System.Linq;
using Scriptable.Datas.FallResources;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Obstacles
{
    [CreateAssetMenu(fileName = "FallObjectsDatabase", menuName = "Databases/FallObstaclesDatabase")]
    public class FallObjectsDatabase: SerializedScriptableObject
    {
        [TabGroup("Resources")] 
        public Dictionary<ResourceCategory, List<FallingResourceConfig>> Resources;

        [TabGroup("Obstacles")] 
        public Dictionary<ObstacleCategory, List<FallObstacleConfig>> Obstacles; 

        private void OnValidate()
        {
            ValidateObstacleUniquenessAndCategoryMatch();
            ValidateResourceUniquenessAndCategoryMatch();
        }

        private void ValidateResourceUniquenessAndCategoryMatch()
        {
            foreach (var entry in Resources)
            {
                var category = entry.Key;
                var resources = entry.Value;
                
                if (resources.GroupBy(x => x.Type).Any(g => g.Count() > 1))
                    Debug.LogError($"[FallObjectsDatabase] Обнаружены дубликаты в категории {category}.", this);
                
                foreach (var resource in resources)
                    if (resource.CategoryType != category)
                        Debug.LogError($"[FallObjectsDatabase] Ресурс {resource.name} не соответствует категории {category}.", this);
            }
        }

        private void ValidateObstacleUniquenessAndCategoryMatch()
        {
            foreach (var entry in Obstacles)
            {
                var category = entry.Key;
                var obstacles = entry.Value;

                if (obstacles.GroupBy(x => x.Type).Any(g => g.Count() > 1))
                    Debug.LogError($"[FallObjectsDatabase] Обнаружены дубликаты в категории {category}.", this);
                
                foreach (var item in obstacles)
                    if (item.CategoryType != category)
                        Debug.LogError($"[FallObjectsDatabase] Ресурс {item.name} не соответствует категории {category}.", this);
            }
        }
    }
}