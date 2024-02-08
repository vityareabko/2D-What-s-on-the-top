using System;
using UnityEngine;
using UnityEngine.UI;

public class StaminaView : MonoBehaviour
{
    [SerializeField] private Slider _staminaSlider;

    public void Initialize(float _minValue, float _maxValue)
    {
        _staminaSlider.minValue = _minValue;
        _staminaSlider.maxValue = _maxValue;
    }

    public void SetStaminaValue(float currentStamina)
    {
        if (currentStamina >= _staminaSlider.maxValue ||  currentStamina <= _staminaSlider.minValue)
            return;

        _staminaSlider.value = currentStamina;
    }

}
