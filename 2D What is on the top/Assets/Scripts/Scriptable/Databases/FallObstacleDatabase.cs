using System.Collections.Generic;
using Scriptable.Datas.FallResources;
using UnityEngine;

namespace Obstacles
{
    [CreateAssetMenu(fileName = "FallObstacleDatabase", menuName = "Databases/FallObstaclesDatabase")]
    public class FallObstacleDatabase: ScriptableObject
    {
        [field: SerializeField] public List<FallObstacleConfig> FallObstacleConfigs { get; private set; }
        [field: SerializeField] public List<FallingResourceConfig> FallingResourceConfigs { get; private set; }
    }
}