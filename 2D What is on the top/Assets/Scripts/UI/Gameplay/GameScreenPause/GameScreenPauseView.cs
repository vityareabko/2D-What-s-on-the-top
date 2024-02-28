using System;
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
        
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _resumeButton;

        public IGameScreenPausePresenter Presentor { get; private set; }
        
        public void InitPresentor(IGameScreenPausePresenter presentor) => Presentor = presentor;
        
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