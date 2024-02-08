using UnityEngine;

[System.Serializable]
public class StaminaData
{
    [field: SerializeField] public float MaxStamina { get; private set; } = 50;
    [field: SerializeField] public float MinStamina { get; private set; } = 0;
}