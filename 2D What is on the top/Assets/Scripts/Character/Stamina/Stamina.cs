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

    public void DrainStamina(float amount)
    {
        if (_currentStamin <= _staminaData.MinStamina) 
            return;
        
        _currentStamin -= amount;
        _gameScreenPresenter.UpdateStamina(_currentStamin);
    }

    public void RegenerateStamina(float amount)
    {
        if (_currentStamin >= _staminaData.MaxStamina)
            return;

        _currentStamin += amount;
        _gameScreenPresenter.UpdateStamina(_currentStamin);
    }

    public bool isEnough() => _currentStamin > _staminaData.MinStamina;
    
}