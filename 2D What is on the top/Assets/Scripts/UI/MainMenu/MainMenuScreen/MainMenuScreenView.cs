using System;
using DG.Tweening;
using Extensions;
using TMPro;
using UI.MVP;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public interface IIMainMenuView : IView <IMainMenuPresenter>
    {
        public void SetCristtalAmount(int amount);
        public void SetCoinsAmount(int amount);
    }

    public class MainMenuScreenView : BaseScreenView, IIMainMenuView
    {
        public override ScreenType ScreenType { get; } = ScreenType.MainMenu;
        
        [Header("RectTransforms")]
        [SerializeField] private RectTransform _topPanel;
        [SerializeField] private RectTransform _bottomPanel;
        [SerializeField] private RectTransform _resourceLevelPanel;
        [SerializeField] private RectTransform _levelPanel;
        [SerializeField] private RectTransform _levels;
        
        [Header("Text")]
        [SerializeField] private TMP_Text _cristalAmount;
        [SerializeField] private TMP_Text _coinsAmount;
        
        [Header("Buttons")]
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _shopSkinButton;
       
        [Header("Toggle")]
        [SerializeField] private Toggle _levelsShowToggle;
        
        public IMainMenuPresenter Presentor { get; private set; }
        
        public void InitPresentor(IMainMenuPresenter presentor) => Presentor = presentor;

        protected override void OnShow()
        {
            base.OnShow();
            _topPanel.AnimateFromOutsideToPosition(_topPanel.anchoredPosition, RectTransformExtensions.Direction.Up);
            _bottomPanel.AnimateFromOutsideToPosition(_bottomPanel.anchoredPosition, RectTransformExtensions.Direction.Down);
            _levelPanel.AnimateFromOutsideToPosition(_levelPanel.anchoredPosition, RectTransformExtensions.Direction.Right);
            _resourceLevelPanel.AnimateFromOutsideToPosition(_resourceLevelPanel.anchoredPosition, RectTransformExtensions.Direction.Left);
        }

        public override void Hide(Action callBack = null)
        {
            if (callBack == null)
            {
                base.Hide();
                return;
            }
            
            int totalAnimations = 3;
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
            _levels.AnimateBackOutsideScreen(RectTransformExtensions.Direction.Right, callback: OnCompleted);
            _resourceLevelPanel.AnimateBackOutsideScreen(RectTransformExtensions.Direction.Right, callback: OnCompleted);
        }

        private void OnEnable()
        {
            _playButton.onClick.AddListener(OnPlayButtonClicked);
            _shopSkinButton.onClick.AddListener(OnShopSkinButtonClicked);
            _levelsShowToggle.onValueChanged.AddListener(OnClickShowLevelsButton);
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveListener(OnPlayButtonClicked);
            _shopSkinButton.onClick.RemoveListener(OnShopSkinButtonClicked);
            _levelsShowToggle.onValueChanged.RemoveListener(OnClickShowLevelsButton);
        }

        private void OnClickShowLevelsButton(bool isOn)
        {
            _levels.DOAnchorPos(_levels.anchoredPosition * -1f, 0.7f).SetEase(Ease.OutBounce);
        }


        public void SetCristtalAmount(int amount) => _cristalAmount.Show(amount);

        public void SetCoinsAmount(int amount) => _coinsAmount.Show(amount);

        private void OnPlayButtonClicked() => Presentor.OnClickedPlayButton();

        private void OnShopSkinButtonClicked() => Presentor.OnClickedShopSkinsButton();
    }
}