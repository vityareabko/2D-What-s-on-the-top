
using UnityEngine;

[System.Serializable]
public class LevelConfigDatas
{
    [field: SerializeField] public int MaxHeightLevel { get; private set; } = 500;
    
    [field: SerializeField] public float MinTimeSpawObstacle { get; private set; } = 1f;
    [field: SerializeField] public float StartTimeSpawmObstacle { get; private set; } = 2.5f;

    [field: SerializeField] public float StartTimeSpawnResource { get; private set; } = 2f;
    [field: SerializeField] public float MinTimeSpawnResource { get; private set; } = 1f;

}
