using DamageNumbersPro;
using Sirenix.OdinInspector;
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
        public void SpawnPopupTextDrainStamina(int value);
    }

    public class GameScreenHUDView : BaseScreenView, IGameSreenView
    {
        public override ScreenType ScreenType { get; } = ScreenType.GameScreenHUD;
        
        [FoldoutGroup("Main")] [SerializeField] private TMP_Text _heightScoreText;
        [FoldoutGroup("Main")] [SerializeField] private Button _pauseButton;
        [FoldoutGroup("Main")] [SerializeField] private Slider _stamina;
        [FoldoutGroup("Main")] [SerializeField] private TMP_Text _amountCoins;
        
        [FoldoutGroup("Popup Text")] [SerializeField] private DamageNumberGUI _drainStaminaPopUpText;
        [FoldoutGroup("Popup Text")] [SerializeField] private Image _sliderCurrentValueStamimIconForPopupText;
        
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

        public void SpawnPopupTextDrainStamina(int value)
        {
            var rectTransformStamina = _sliderCurrentValueStamimIconForPopupText.GetComponent<RectTransform>();
            Vector2 anchoredPosition = new Vector2(rectTransformStamina.anchoredPosition.x, rectTransformStamina.anchoredPosition.y + .5f);
            _drainStaminaPopUpText.SpawnGUI(rectTransformStamina, anchoredPosition, value);
        }
    }
}