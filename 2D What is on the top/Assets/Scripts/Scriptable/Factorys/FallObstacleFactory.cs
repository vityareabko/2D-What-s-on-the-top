using System;
using System.Collections.Generic;
using System.Linq;
using Scriptable.Datas.FallResources;
using UnityEngine;


namespace Obstacles
{
    [CreateAssetMenu(fileName = "FallObstacleFactory", menuName = "Factory/FallObjectBase")]
    public class FallObstacleFactory : ScriptableObject
    {
        [SerializeField] private FallObjectsDatabase _objectsDatabase;
        
        public FallObstacle Get(ObstacleType type, Transform parent)
        {
            var getRandomObstacleCategory = (ObstacleCategory)UnityEngine.Random.Range(0, Enum.GetValues(typeof(ObstacleCategory)).Length);
            
            var getListByCategoriesTypes = _objectsDatabase.Obstacles[getRandomObstacleCategory];
            
            var config = getListByCategoriesTypes.First(t => t.Type == type);
            
            var instance = Instantiate(config.Prefab, parent);
            instance.Initialize(config.Speed, config.StaminaDrainRateForColision);
            
            return instance;
        }
        
        public FallResource Get(List<FallingResourceConfig> availableResourcesForSpawn, Transform parent)
        {
            var config = GetRandomResourceByCategory(availableResourcesForSpawn);
            var instance = Instantiate(config.Prefab, parent);
            instance.Initialize(config.Speed, config.Type, config.CategoryType);
            
            return instance;
        }

        private FallingResourceConfig GetRandomResourceByCategory(List<FallingResourceConfig> availableResourcesForSpawn)
        {
            var index = UnityEngine.Random.Range(0, availableResourcesForSpawn.Count);
            return availableResourcesForSpawn[index];
        }
    }
}