using UnityEngine;

[System.Serializable]
public class CharacterData
{
    [field: SerializeField] public float DefaultSpeed { get; private set; } = 4f;
    [field: SerializeField] public float MaxVerticalSpeed { get; private set; } = 8f;
    [field: SerializeField] public float MinVerticalSpeed { get; private set; } = 0.4f;
    [field: SerializeField] public float DecelerationRate { get; private set; } = 1f;
    [field: SerializeField] public float AccelerationRate { get; private set; } = 1f;
    [field: SerializeField] public float JumpForce { get; private set; } = 12f;

    [field: SerializeField] public StaminaData _staminaData { get; private set; }
    [field: SerializeField] public float StaminaDrainRateRunning { get; private set; } = 0.2f;
    [field: SerializeField] public float StaminaDrainRateRoll { get; private set; } = 0.4f;
    [field: SerializeField] public float StaminaDrainRateJumping { get; private set; } = 0.3f;
    [field: SerializeField] public float StaminaDrainRateWalking { get; private set; } = 0.1f;
}