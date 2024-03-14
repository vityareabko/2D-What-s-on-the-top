using System;
using DG.Tweening;
using Extensions;
using TMPro;
using UI.MVP;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public interface IGameScreenDefeatView: IView<IGameScreenDefeatPresenter>
    {
    }

    public class GameScreenDefeatView : BaseScreenView, IGameScreenDefeatView
    {
        public override ScreenType ScreenType { get; } = ScreenType.GameScreenDefeat;

        [Header("RectTransforms")] 
        [SerializeField] private RectTransform _defeatPanelContent;
        
        [Header("Text")]
        [SerializeField] private TMP_Text _amountConis;
        [SerializeField] private TMP_Text _descriptionText;
        
        [Header("Buttons")]
        [SerializeField] private Button _x2WatchAdsButton;
        [SerializeField] private Button _homeButton;
        
        public IGameScreenDefeatPresenter Presentor { get; private set; }

        protected override void OnShow()
        {
            base.OnShow();
            
            _defeatPanelContent.AnimateFromOutsideToPosition(_defeatPanelContent.anchoredPosition, RectTransformExtensions.Direction.Up, 0.6f);
        }

        public override void Hide(Action callBack = null)
        {
            if (callBack == null)
            {
                base.Hide(callBack);
                return;
            }
            
            _defeatPanelContent.AnimateBackOutsideScreen(RectTransformExtensions.Direction.Right, callback: () =>
            {
                callBack?.Invoke();
                base.Hide(callBack);
            });
        }

        private void OnEnable()
        {
            _x2WatchAdsButton.onClick.AddListener(OnX2WatchAdsButton);
            _homeButton.onClick.AddListener(OnHomeButton);
            _descriptionText.text = SelectRandomPhrace();
        }

        private void OnDisable()
        {
            _x2WatchAdsButton.onClick.RemoveListener(OnX2WatchAdsButton);
            _homeButton.onClick.RemoveListener(OnHomeButton);
        }

        public void InitPresentor(IGameScreenDefeatPresenter presentor) => Presentor = presentor;

        public void SetCoins(int amount) => _amountConis.text = amount.ToString();
       
        private void OnHomeButton() => Presentor.OnHomeButtonClicked();
        
        private void OnX2WatchAdsButton() => Presentor.OnX2RewardWatchButton();

        private string SelectRandomPhrace()
        {
            string[] phrases = new[]
            {
                "Looks like the well was too deep this time. Try again?",
                "Falling down is just a step back, not the end. Climb up!",
                "You hit rock bottom, but the only way now is up!",
                "The well is deep, but your determination is deeper. Go again!",
                "Don't let the darkness hold you back. The sky is still above!",
                "Every fall is a lesson. What did you learn this time?",
                "You might have slipped, but don't let it dampen your spirit!",
                "The climb is tough, but so are you. Ready for another go?",
                "This well can't contain you forever. Break free!",
                "Gravity might have won this round, but the game isn't over yet.",
                "Maybe you should upgrade yourself to conquer this peak?",
            };

            return phrases[Random.Range(0, phrases.Length - 1)];
        }

    }
}
