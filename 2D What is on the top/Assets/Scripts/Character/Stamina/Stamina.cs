using UI;

public class Stamina
{
    private StaminaData _staminaData;
    private GameScreenHUDPresenter gameScreenHUDPresenter;
    
    private float _currentStamin;

    public Stamina(CharacterData characterData, GameScreenHUDPresenter gameScreenHUDPresenter)
    {
        _staminaData = characterData.StaminaData;
        _currentStamin = _staminaData.MaxStamina;
        this.gameScreenHUDPresenter = gameScreenHUDPresenter;
    }

    public void DrainRateStaminaRun(float deltaTime) => DrainRateStamina(_staminaData.StaminaDrainRateRunning * deltaTime);
    public void DrainRateStaminaWalking(float deltaTime) => DrainRateStamina(_staminaData.StaminaDrainRateWalking * deltaTime);
    public void DrainRateStaminaJump() => DrainRateStamina(_staminaData.StaminaDrainRateJumping);
    public void DrainRateStaminaUpwardRoll() => DrainRateStamina(_staminaData.StaminaDrainRateRoll);
    public void DrainRateStaminaForObstaclesCollision() => DrainRateStamina(_staminaData.StaminaDrainObstacleCollision);
    
    public bool isEnough() => _currentStamin > _staminaData.MinStamina;

    private void DrainRateStamina(float amount)
    {
        if (_currentStamin <= _staminaData.MinStamina) 
            return;
        
        _currentStamin -= amount;
        gameScreenHUDPresenter.UpdateStamina(_currentStamin);
    }

    private void RegenerateStamina(float amount)
    {
        if (_currentStamin >= _staminaData.MaxStamina)
            return;

        _currentStamin += amount;
        gameScreenHUDPresenter.UpdateStamina(_currentStamin);
    }
}