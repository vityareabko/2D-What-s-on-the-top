using System;
using UI.MVP;
using Zenject;

namespace UI.GameScreenPause
{
    public interface IGameScreenPausePresenter : IPresenter<IGameScreenPauseVIew>
    {
        public event Action OnRestartGameClicked;
        public event Action OnResumeGameClicked;
        public void OnRestartGameButtonClicked();
        public void OnResumeGameButtondClicked();
        
    }

    public class GameScreenPausePresenter : IGameScreenPausePresenter
    {
        public event Action OnRestartGameClicked;
        public event Action OnResumeGameClicked;
        
        public IGameScreenPauseVIew View { get; }
        public IGameScreenPresenter _gameScreenPresenter;

        public bool _isInit = false;

        [Inject] public GameScreenPausePresenter(IGameScreenPauseVIew view)
        {
            View = view;
            
            Init();
        }

        public void Show() => View.Show();
        public void Hide() => View.Hide();
        
        public void Init()
        {
            if (_isInit)
                return;

            View.InitPresentor(this);
            _isInit = true;
        }


        public void OnRestartGameButtonClicked()
        {
            // снять паузу и продолжыть игру
            OnRestartGameClicked?.Invoke();
            View.Hide();
        }

        public void OnResumeGameButtondClicked()
        {
            // начать игру заново
            OnResumeGameClicked?.Invoke();
            View.Hide(); 
        }
    }
}