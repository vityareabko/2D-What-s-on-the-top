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
        
        [SerializeField] private Button _restatrButton;
        [SerializeField] private Button _resumeButton;

        public IGameScreenPausePresenter Presentor { get; private set; }
        
        public void InitPresentor(IGameScreenPausePresenter presentor) => Presentor = presentor;
        
        private void OnEnable()
        {
            _restatrButton.onClick.AddListener(OnRestartGameButton);
            _resumeButton.onClick.AddListener(OnResumeGameButtond);
        }

        private void OnDisable()
        {
            _restatrButton.onClick.RemoveListener(OnRestartGameButton);
            _resumeButton.onClick.RemoveListener(OnResumeGameButtond);
        }

        private void OnResumeGameButtond() => Presentor.OnResumeGameButtondClicked();
        

        private void OnRestartGameButton() => Presentor.OnRestartGameButtonClicked();
        

    }
}