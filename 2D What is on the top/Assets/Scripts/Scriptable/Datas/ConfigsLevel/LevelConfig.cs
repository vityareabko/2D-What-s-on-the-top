using UnityEngine;

namespace Scriptable.Datas.ConfigsLevel
{
    [CreateAssetMenu (fileName = "ConfigLevel", menuName = "Config/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [field: SerializeField] public int MaxHeightLevel { get; private set; }
        [field: SerializeField] public float MinTimeSpawObstacle { get; private set; }
        [field: SerializeField] public float StartTimeSpawmObstacle { get; private set; }



        [field: SerializeField] public float StartTimeSpawnResource { get; private set; }
        [field: SerializeField] public float MinTimeSpawnResource { get; private set; }

    }
}