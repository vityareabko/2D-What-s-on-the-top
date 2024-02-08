
using System;

public class Stamina
{
    public event Action<float> StaminaChange;

    private StaminaData _staminaData;
    
    private float _currentStamin;

    public Stamina(StaminaData staminaData)
    {
        _staminaData = staminaData;
        _currentStamin = _staminaData.MaxStamina;
    }

    public void DrainStamina(float amount)
    {
        if (_currentStamin <= _staminaData.MinStamina) 
            return;
        
        _currentStamin -= amount;
        StaminaChange?.Invoke(_currentStamin);
    }

    public void RegenerateStamina(float amount)
    {
        if (_currentStamin >= _staminaData.MaxStamina)
            return;

        _currentStamin += amount;
        StaminaChange?.Invoke(_currentStamin);
    }

    public bool isEnough() => _currentStamin > _staminaData.MinStamina;
}