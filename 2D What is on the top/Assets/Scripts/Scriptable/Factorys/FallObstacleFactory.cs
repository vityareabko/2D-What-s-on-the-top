using System.Collections.Generic;
using System.Linq;
using Scriptable.Datas.FallResources;
using UnityEngine;
using UnityEngine.Serialization;

namespace Obstacles
{
    [CreateAssetMenu(fileName = "FallObstacleFactory", menuName = "Factory/FallObjectBase")]
    public class FallObstacleFactory : ScriptableObject
    {
        [FormerlySerializedAs("_obstacleDatabase")] [SerializeField] private FallObjectsDatabase objectsDatabase;
        
        public FallObstacle Get(FallingObstaclesType type, Transform parent)
        {
            var config = objectsDatabase.FallObstacleConfigs.First(t => t.Type == type);
            var instance = Instantiate(config.Prefab, parent);
            instance.Initialize(config.Speed, config.StaminaDrainRateForColision);
            
            
            return instance;
        }
        
        // public FallResource Get(ResourceTypes type, Transform parent)
        public FallResource Get(ResourceCategory categoryType, Transform parent)
        {
            var config = GetRandomResourceByCategory(categoryType);
            // var cofig = objectsDatabase.FallingResourceConfigs.First(t => t.Type == type);
            
            var instance = Instantiate(config.Prefab, parent);
            instance.Initialize(config.Speed, config.Type, config.CategoryType);
            
            return instance;
        }

        private FallingResourceConfig GetRandomResourceByCategory(ResourceCategory category)
        {
            var categoryResource = objectsDatabase.FallingResourceConfigs.Where(t => t.CategoryType == category).ToList();
            var index = UnityEngine.Random.Range(0, categoryResource.Count);
            return categoryResource[index];
        }
    }
}