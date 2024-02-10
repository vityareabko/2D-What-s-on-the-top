using UnityEngine;


[CreateAssetMenu(fileName = "PlayerStats", menuName = "Stats/PlayerStats")]
public class CharacterData : ScriptableObject
{
    [field: SerializeField] public float DefaultSpeed { get; private set; } = 4f;
    [field: SerializeField] public float MaxVerticalSpeed { get; private set; } = 8f;
    [field: SerializeField] public float MinVerticalSpeed { get; private set; } = 0.4f;
    [field: SerializeField] public float DecelerationRate { get; private set; } = 1f;
    
    [field: SerializeField] public float AccelerationRate { get; private set; } = 1f;
    [field: SerializeField] public float JumpForce { get; private set; } = 12f;
    [field: SerializeField] public StaminaData StaminaData { get; private set; }
    
}