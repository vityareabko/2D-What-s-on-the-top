using System;
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
        public Dictionary<ResourceCategory, List<FallingResourceConfig>> Resources;
        public Dictionary<ObstacleCategory, List<FallObstacleConfig>> Obstacles;

        private void OnValidate()
        {
            ValidateObstacleUniquenessAndCategoryMatch();
            ValidateResourceUniquenessAndCategoryMatch();
        }
        
        [Button("Initialize Dictionaries")]
        private void InitializeDictionaries()
        {
            InitializeResourcesCategories();
            InitializeObstacleCategories();
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
        
        
        private void InitializeObstacleCategories()
        {
            if (Obstacles == null)
            {
                Obstacles = new Dictionary<ObstacleCategory, List<FallObstacleConfig>>();
            
                foreach (ObstacleCategory category in System.Enum.GetValues(typeof(ObstacleCategory)))
                {
                    Obstacles[category] = new List<FallObstacleConfig>();
                }
            }
        
        }
        private void InitializeResourcesCategories()
        {
            if (Resources == null)
            {
                Resources = new Dictionary<ResourceCategory, List<FallingResourceConfig>>();
            
                foreach (ResourceCategory category in System.Enum.GetValues(typeof(ResourceCategory)))
                {
                    Resources[category] = new List<FallingResourceConfig>();
                }
            }
        }
    }
}