using UnityEngine;

[System.Serializable]
public class StaminaData
{
    [field: SerializeField] public float MaxStamina { get; private set; } = 50;
    [field: SerializeField] public float MinStamina { get; private set; } = 0;
    
    [field: SerializeField] public float StaminaDrainRateRunning { get; private set; } = 0.2f;
    [field: SerializeField] public float StaminaDrainRateRoll { get; private set; } = 0.4f;
    [field: SerializeField] public float StaminaDrainRateJumping { get; private set; } = 0.3f;
    [field: SerializeField] public float StaminaDrainRateWalking { get; private set; } = 0.1f;

}