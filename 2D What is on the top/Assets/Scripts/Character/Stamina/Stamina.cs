using System;using UI;

public class Stamina : IDisposable
{
    private StaminaData _staminaData;
    private GameScreenHUDPresenter _gameScreenHUDPresenter;
    
    private float _currentStamin;

    public Stamina(PlayerConfig playerConfig, GameScreenHUDPresenter gameScreenHUDPresenter)
    {
        _staminaData = playerConfig.StaminaData;
        _gameScreenHUDPresenter = gameScreenHUDPresenter;
        Initialize();
        
        EventAggregator.Subscribe<PopupTextDrainStaminEvent>(OnColisionObstacle); 
    }
    
    public void Dispose() => EventAggregator.Unsubscribe<PopupTextDrainStaminEvent>(OnColisionObstacle);

    public void Initialize()
    {
        _currentStamin = _staminaData.MaxStamina;
        DrainRateStamina(0f);
    }

    public void DrainRateStaminaRun(float deltaTime) => DrainRateStamina(_staminaData.StaminaDrainRateRunning * deltaTime);
    public void DrainRateStaminaWalking(float deltaTime) => DrainRateStamina(_staminaData.StaminaDrainRateWalking * deltaTime);
    public void DrainRateStaminaJump() => DrainRateStamina(_staminaData.StaminaDrainRateJumping);
    public void DrainRateStaminaUpwardRoll() => DrainRateStamina(_staminaData.StaminaDrainRateRoll);
    
    public bool isEnough() => _currentStamin > _staminaData.MinStamina;

    private void DrainRateStamina(float amount)
    {
        if (_currentStamin <= _staminaData.MinStamina) 
            return;
        
        _currentStamin -= amount;
        _gameScreenHUDPresenter.UpdateStamina(_currentStamin);
    }

    private void RegenerateStamina(float amount)
    {
        if (_currentStamin >= _staminaData.MaxStamina)
            return;

        _currentStamin += amount;
        _gameScreenHUDPresenter.UpdateStamina(_currentStamin);
    }
    
    private void OnColisionObstacle(object sender, PopupTextDrainStaminEvent eventData) => DrainRateStamina(eventData.DrainAmount);
    
}