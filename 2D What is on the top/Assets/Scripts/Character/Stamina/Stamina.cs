using UI;

public class Stamina
{
    private StaminaData _staminaData;
    private GameScreenPresenter _gameScreenPresenter;
    
    private float _currentStamin;

    public Stamina(CharacterData characterData, GameScreenPresenter gameScreenPresenter)
    {
        _staminaData = characterData.StaminaData;
        _currentStamin = _staminaData.MaxStamina;
        _gameScreenPresenter = gameScreenPresenter;
    }

    public void DrainStaminaRun(float deltaTime) => DrainStamina(_staminaData.StaminaDrainRateRunning * deltaTime);
    public void DrainStaminaWalking(float deltaTime) => DrainStamina(_staminaData.StaminaDrainRateWalking * deltaTime);
    public void DrainStaminaJump() => DrainStamina(_staminaData.StaminaDrainRateJumping);
    public void DrainStaminaUpwardRoll() => DrainStamina(_staminaData.StaminaDrainRateRoll);
    
    public bool isEnough() => _currentStamin > _staminaData.MinStamina;

    private void DrainStamina(float amount)
    {
        if (_currentStamin <= _staminaData.MinStamina) 
            return;
        
        _currentStamin -= amount;
        _gameScreenPresenter.UpdateStamina(_currentStamin);
    }

    private void RegenerateStamina(float amount)
    {
        if (_currentStamin >= _staminaData.MaxStamina)
            return;

        _currentStamin += amount;
        _gameScreenPresenter.UpdateStamina(_currentStamin);
    }

    
}