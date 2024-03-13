using System;
using DG.Tweening;
using Extensions;
using UI.MVP;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameScreenPause
{
    public interface IGameScreenPauseVIew : IView<IGameScreenPausePresenter>
    {
        
    }

    public class GameScreenPauseView : BaseScreenView, IGameScreenPauseVIew
    {
        public override ScreenType ScreenType { get; } = ScreenType.GameScreenPause;

        [Header("RectTransform")] 
        [SerializeField] private RectTransform _rectTransformResumeButton;
        [SerializeField] private RectTransform _rectTransformMainMenuButton;
        
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _resumeButton;

        public IGameScreenPausePresenter Presentor { get; private set; }
        
        public void InitPresentor(IGameScreenPausePresenter presentor) => Presentor = presentor;

        protected override void OnShow()
        {
            base.OnShow();
            
            _rectTransformResumeButton.AnimateFromOutsideToPosition(_rectTransformResumeButton.anchoredPosition,  RectTransformExtensions.Direction.Up);
            _rectTransformMainMenuButton.AnimateFromOutsideToPosition(_rectTransformMainMenuButton.anchoredPosition, RectTransformExtensions.Direction.Down);
        }

        public override void Hide(Action callBack)
        {
            if (callBack == null)
            {
                base.Hide(callBack);
                return;
            }
            
            int totalAnimations = 2;
            int countAnimationCompleted = 0;
            
            Action OnCompleteAnimation = () =>
            {
                countAnimationCompleted++;
                if (totalAnimations == countAnimationCompleted)
                {
                    callBack?.Invoke();
                    Debug.Log("@@@@Wtf");
                    base.Hide(callBack);
                }
            };
            
            _rectTransformResumeButton.AnimateBackOutsideScreen(RectTransformExtensions.Direction.Up, callback: OnCompleteAnimation);
            _rectTransformMainMenuButton.AnimateBackOutsideScreen(RectTransformExtensions.Direction.Down, callback: OnCompleteAnimation);
            
        }

        private void OnEnable()
        {
            _mainMenuButton.onClick.AddListener(OnMainMenuButton);
            _resumeButton.onClick.AddListener(OnResumeGameButton);
        }

        private void OnDisable()
        {
            _mainMenuButton.onClick.RemoveListener(OnMainMenuButton);
            _resumeButton.onClick.RemoveListener(OnResumeGameButton);
        }

        private void OnResumeGameButton() => Presentor.OnResumeGameButtondClicked();
        
        private void OnMainMenuButton() => Presentor.OnMainMenuButtonClicked();
        

    }
}