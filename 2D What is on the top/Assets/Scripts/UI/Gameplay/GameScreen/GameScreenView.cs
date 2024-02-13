using TMPro;
using UI.MVP;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public interface IGameSreenView : IView<IGameScreenPresenter>
    {
        public void Initialize(StaminaData data);
        public void SetHightScore(int score);
        public void SetStaminaValue(float currentStamina);
        public void SetAmountCoins(int value);
    }

    public class GameScreenView : BaseScreenView, IGameSreenView
    {
        public override ScreenType ScreenType { get; } = ScreenType.GameScreen;
        
        [SerializeField] private TMP_Text _heightScoreText;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Slider _stamina;
        
        [SerializeField] private TMP_Text _amountCoins;
        
        public IGameScreenPresenter Presentor { get; private set; }
        
        public void InitPresentor(IGameScreenPresenter presentor) => Presentor = presentor;
        
        public void Initialize(StaminaData data)
        {
            _stamina.minValue = data.MinStamina;
            _stamina.maxValue = data.MaxStamina;
            _stamina.value = _stamina.maxValue;

            _amountCoins.text = "0";
        }

        private void OnEnable() => _pauseButton.onClick.AddListener(OnButtonPauseClick);
        
        private void OnDisable() => _pauseButton.onClick.RemoveListener(OnButtonPauseClick);
        
        private void OnButtonPauseClick() => Presentor.OnPauseButtonClicked();
        
        public void SetHightScore(int score) => _heightScoreText.text = $"{score.ToString()}m";
        
        public void SetStaminaValue(float currentStamina) => _stamina.value = currentStamina;

        public void SetAmountCoins(int value) => _amountCoins.text = value.ToString();
        
    }
}