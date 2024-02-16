using UnityEngine;

namespace Obstacles
{
    [System.Serializable]
    public class FallObstacleConfig
    {
        [field: SerializeField] public FallingObstaclesType Type { get; private set; }
        [field: SerializeField] public FallObstacle Prefab { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
    }
}