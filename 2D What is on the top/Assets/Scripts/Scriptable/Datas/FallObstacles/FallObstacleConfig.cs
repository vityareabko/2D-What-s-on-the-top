using UnityEngine;

namespace Obstacles
{
    [CreateAssetMenu(fileName = "ObstacleConfig", menuName = "Config/ObstacleConfig")]
    public class FallObstacleConfig : ScriptableObject
    {
        [field: SerializeField] public ObstacleCategory CategoryType;
        [field: SerializeField] public ObstacleType Type { get; set; }
        [field: SerializeField] public FallObstacle Prefab { get; private set; }
        [field: SerializeField] public float Speed { get; set; }
        [field: SerializeField] public int StaminaDrainRateForColision { get; set; }

    }
}