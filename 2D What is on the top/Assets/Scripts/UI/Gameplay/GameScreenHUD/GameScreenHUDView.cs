using System;
using DamageNumbersPro;
using Extensions;
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

        [Header("Rect Transforms")] 
        [SerializeField] private RectTransform _topPanel;
        [SerializeField] private RectTransform _bottomPanel;
        
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

        protected override void OnShow()
        {
            base.OnShow();
            _topPanel.AnimateFromOutsideToPosition(_topPanel.anchoredPosition, RectTransformExtensions.Direction.Up);
            _bottomPanel.AnimateFromOutsideToPosition(_bottomPanel.anchoredPosition, RectTransformExtensions.Direction.Down);
            // _topPanel.AnimateToPosition(_topPanel.anchoredPosition, flipX: false, callback: () => Debug.Log("COCOCO"));
            // _bottomPanel.AnimateToPosition(_bottomPanel.anchoredPosition, flipX: false);
        }

        public override void Hide(Action callBack = null)
        {
            if (callBack == null)
            {
                base.Hide();
                return;
            }
            
            int totalAnimations = 2;
            int countAnimationCompleted = 0;

            Action OnCompleted = () =>
            {
                countAnimationCompleted++;
                if (countAnimationCompleted == totalAnimations)
                {
                    callBack?.Invoke();
                    base.Hide(callBack);
                }
            };
            
            
            _topPanel.AnimateBackOutsideScreen(RectTransformExtensions.Direction.Up, callback: OnCompleted);
            _bottomPanel.AnimateBackOutsideScreen(RectTransformExtensions.Direction.Down, callback: OnCompleted);
            // _topPanel.AnimateToHidePosition(new Vector2(0, _topPanel.anchoredPosition.y * -1f), flipX: false, callback: OnCompleted);
            // _bottomPanel.AnimateToHidePosition(new Vector2(0, _bottomPanel.anchoredPosition.y * -1f), flipX: false, callback: OnCompleted);
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