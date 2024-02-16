using System.Collections.Generic;
using UnityEngine;

namespace Obstacles
{
    [CreateAssetMenu(fileName = "FallObstacleDatabase", menuName = "Databases/FallObstaclesDatabase")]
    public class FallObstacleDatabase: ScriptableObject
    {
        [field: SerializeField] public List<FallObstacleConfig> FallObstacleConfigs;
    }
}