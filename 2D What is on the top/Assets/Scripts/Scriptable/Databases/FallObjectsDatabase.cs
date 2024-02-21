using System.Collections.Generic;
using System.Linq;
using Scriptable.Datas.FallResources;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = System.Random;

namespace Obstacles
{
    [CreateAssetMenu(fileName = "FallObjectsDatabase", menuName = "Databases/FallObstaclesDatabase")]
    public class FallObjectsDatabase: ScriptableObject
    {
        [field: SerializeField, ListDrawerSettings(CustomAddFunction = "AddUniqueFallObstacleConfig")]  public List<FallObstacleConfig> FallObstacleConfigs { get; private set; }
        
        [field: SerializeField, ListDrawerSettings(CustomAddFunction = "AddUniqueFallingResourceConfig")] public List<FallingResourceConfig> FallingResourceConfigs { get; private set; }

        private void OnValidate()
        {
            if (FallObstacleConfigs.GroupBy(x => x.Type).Any(g => g.Count() > 1))
                Debug.LogError($"[FallObjectsDatabase] Обнаружены дубликаты в FallObstacleConfigs.", this);
            
            if (FallingResourceConfigs.GroupBy(x => x.Type).Any(g => g.Count() > 1))
                Debug.LogError($"[FallObjectsDatabase] Обнаружены дубликаты в FallingResourceConfigs.", this);
        }
        
        private void AddUniqueFallObstacleConfig()
        {
            var existingTypes = FallObstacleConfigs.Select(c => c.Type).ToList();
            var allTypes = System.Enum.GetValues(typeof(FallingObstaclesType)).Cast<FallingObstaclesType>();

            var availableType = allTypes.FirstOrDefault(t => existingTypes.Contains(t) == false);

            if (availableType != default(FallingObstaclesType))
                FallObstacleConfigs.Add(new FallObstacleConfig { Type = availableType });
        }
        
        private void AddUniqueFallingResourceConfig()
        {
            var existingTypes = FallingResourceConfigs.Select(c => c.Type).ToList();
            var allTypes = System.Enum.GetValues(typeof(ResourceTypes)).Cast<ResourceTypes>();

            var availableType = allTypes.FirstOrDefault(t => existingTypes.Contains(t) == false);
            
            if (availableType != default(ResourceTypes))
                FallingResourceConfigs.Add(new FallingResourceConfig { Type = availableType });
        }

        [Button] private void SetSpeedObstacleObjects(int speed)
        {
            foreach (var item in FallObstacleConfigs)
                item.Speed = speed;
        }
        
        [Button] private void SetSpeedResourceObjects(int speed)
        {
            foreach (var item in FallingResourceConfigs)
                item.Speed = speed;
        }

        [Button] private void SetRandomStaminaDrainForCollisionAboutObstacle()
        {
            foreach (var fallObstacleConfig in FallObstacleConfigs)
            {
                fallObstacleConfig.StaminaDrainRateForColision = UnityEngine.Random.Range(1, 4);
            }
        }
        
        [Button] private void SetRandomSpeedFall()
        {
            foreach (var fallObstacleConfig in FallObstacleConfigs)
                fallObstacleConfig.Speed = UnityEngine.Random.Range(7,12);

            foreach (var fallingResourceConfig in FallingResourceConfigs)
                fallingResourceConfig.Speed = UnityEngine.Random.Range(5, 9);
        }
    }
}