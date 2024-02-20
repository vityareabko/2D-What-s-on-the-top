using System.Collections.Generic;
using System.Linq;
using Scriptable.Datas.FallResources;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Obstacles
{
    [CreateAssetMenu(fileName = "FallObstacleDatabase", menuName = "Databases/FallObstaclesDatabase")]
    public class FallObstacleDatabase: ScriptableObject
    {
        [field: SerializeField] public List<FallObstacleConfig> FallObstacleConfigs { get; private set; }
        [field: SerializeField] public List<FallingResourceConfig> FallingResourceConfigs { get; private set; }

        private void OnValidate()
        {
            // Проверка FallObstacleConfigs на дубликаты
            if (FallObstacleConfigs.GroupBy(x => x.Type).Any(g => g.Count() > 1))
            {
                Debug.LogError($"[FallObstacleDatabase] Обнаружены дубликаты в FallObstacleConfigs.", this);
            }

            // Проверка FallingResourceConfigs на дубликаты
            if (FallingResourceConfigs.GroupBy(x => x.Type).Any(g => g.Count() > 1))
            {
                Debug.LogError($"[FallObstacleDatabase] Обнаружены дубликаты в FallingResourceConfigs.", this);

            }
        }

        [Button]
        private void SetSpeedObstacleObjects(int speed)
        {
            foreach (var item in FallObstacleConfigs)
                item.Speed = speed;
        }
        
        [Button]
        private void SetSpeedResourceObjects(int speed)
        {
            foreach (var item in FallingResourceConfigs)
                item.Speed = speed;
        }
    }
}