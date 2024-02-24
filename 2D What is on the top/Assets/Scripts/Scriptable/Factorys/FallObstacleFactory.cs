using System.Collections.Generic;
using Scriptable.Datas.FallResources;
using UnityEngine;


namespace Obstacles
{
    [CreateAssetMenu(fileName = "FallObstacleFactory", menuName = "Factory/FallObjectBase")]
    public class FallObstacleFactory : ScriptableObject
    {

        public FallObstacle Get(List<FallObstacleConfig> availableObstaclesForSpawn, Transform parent)
        {
            var config = GetRandomAvailableObstacle(availableObstaclesForSpawn);
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

        private FallObstacleConfig GetRandomAvailableObstacle(List<FallObstacleConfig> availableObstaclesForSpawn)
        {
            var index = UnityEngine.Random.Range(0, availableObstaclesForSpawn.Count);
            return availableObstaclesForSpawn[index];
        }
    }
}