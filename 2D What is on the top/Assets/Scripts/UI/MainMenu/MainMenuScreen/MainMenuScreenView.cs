using System;
using Extensions;
using TMPro;
using UI.MVP;
using UnityEngine;
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
        
        [Header("Text")]
        [SerializeField] private TMP_Text _cristalAmount;
        [SerializeField] private TMP_Text _coinsAmount;
        
        [Header("Buttons")]
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _shopSkinButton;
        
        public IMainMenuPresenter Presentor { get; private set; }
        
        public void InitPresentor(IMainMenuPresenter presentor) => Presentor = presentor;

        protected override void OnShow()
        {
            base.OnShow();
            // _topPanel.AnimateToPosition(_topPanel.anchoredPosition, flipX: false);
            // _bottomPanel.AnimateToPosition(_bottomPanel.anchoredPosition, flipX: false);
            _topPanel.AnimateFromOutsideToPosition(_topPanel.anchoredPosition, RectTransformExtensions.Direction.Up);
            _bottomPanel.AnimateFromOutsideToPosition(_bottomPanel.anchoredPosition, RectTransformExtensions.Direction.Down);
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

        private void OnEnable()
        {
            _playButton.onClick.AddListener(OnPlayButtonClicked);
            _shopSkinButton.onClick.AddListener(OnShopSkinButtonClicked);
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveListener(OnPlayButtonClicked);
            _shopSkinButton.onClick.RemoveListener(OnShopSkinButtonClicked);
        }

        public void SetCristtalAmount(int amount) => _cristalAmount.Show(amount);

        public void SetCoinsAmount(int amount) => _coinsAmount.Show(amount);

        private void OnPlayButtonClicked() => Presentor.OnClickedPlayButton();

        private void OnShopSkinButtonClicked() => Presentor.OnClickedShopSkinsButton();
    }
}