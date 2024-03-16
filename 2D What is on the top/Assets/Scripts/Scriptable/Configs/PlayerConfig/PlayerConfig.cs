using UnityEngine;


[CreateAssetMenu(fileName = "PlayerStats", menuName = "Stats/PlayerStats")]
public class PlayerConfig : ScriptableObject
{
    [field: SerializeField] public float RunSpeed { get; private set; } = 4f;
    [field: SerializeField] public float RollVerticalSpeed { get; private set; } = 8f;
    [field: SerializeField] public float WalkVerticalSpeed { get; private set; } = 0.4f;
    [field: SerializeField] public float JumpForce { get; private set; } = 12f;
    
    [field: SerializeField] public float DecelerationRate { get; private set; } = 1f;
    [field: SerializeField] public float AccelerationRate { get; private set; } = 1f;
    
    [field: SerializeField] public StaminaData StaminaData { get; private set; }
    //
    // [field: SerializeField] public LevelType CurrentLevelSpawnPoint { get; private set; }
    //
    // public void SetCurrentLevel(LevelType type) => CurrentLevelSpawnPoint = type;
}