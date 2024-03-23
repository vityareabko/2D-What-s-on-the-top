using System;using UI;

public class Stamina : IDisposable
{
    private PlayerStats _playerData;
    private GameScreenHUDPresenter _gameScreenHUDPresenter;
    
    private float _currentStamin;

    public Stamina(PlayerStats playerStats, GameScreenHUDPresenter gameScreenHUDPresenter)
    {
        _playerData = playerStats;
        _gameScreenHUDPresenter = gameScreenHUDPresenter;
        Initialize();
        
        EventAggregator.Subscribe<PopupTextDrainStaminEvent>(OnColisionObstacle); 
    }
    
    public void Dispose() => EventAggregator.Unsubscribe<PopupTextDrainStaminEvent>(OnColisionObstacle);

    public void Initialize()
    {
        _currentStamin = _playerData.GetMaxStamina();
        DrainRateStamina(0f);
    }

    public void DrainRateStaminaRun(float deltaTime) => DrainRateStamina(_playerData.GetStaminaDrainRateRunning() * deltaTime);
    public void DrainRateStaminaWalking(float deltaTime) => DrainRateStamina(_playerData.StaminaDrainRateWalking * deltaTime);
    public void DrainRateStaminaJump() => DrainRateStamina(_playerData.GetStaminaDrainRateJumping());
    public void DrainRateStaminaUpwardRoll() => DrainRateStamina(_playerData.GetStaminaDrainRateRoll());
    
    public bool isEnough() => _currentStamin > 0;

    private void DrainRateStamina(float amount)
    {
        if (_currentStamin <= 0) 
            return;
        
        _currentStamin -= amount;
        _gameScreenHUDPresenter.UpdateStamina(_currentStamin);
    }

    private void RegenerateStamina(float amount)
    {
        if (_currentStamin >= _playerData.GetMaxStamina())
            return;

        _currentStamin += amount;
        _gameScreenHUDPresenter.UpdateStamina(_currentStamin);
    }
    
    private void OnColisionObstacle(object sender, PopupTextDrainStaminEvent eventData) => DrainRateStamina(eventData.DrainAmount);
    
}