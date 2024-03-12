using System;
using UI.MVP;
using Zenject;

namespace UI.GameScreenPause
{
    public interface IGameScreenPausePresenter : IPresenter<IGameScreenPauseVIew>
    {
        public event Action OnMainMenuButtonIsClicked;
        public event Action OnResumeGameButtonIsClicked; 
        public void OnMainMenuButtonClicked();
        public void OnResumeGameButtondClicked();
        
    }

    public class GameScreenPausePresenter : IGameScreenPausePresenter
    {
        public event Action OnMainMenuButtonIsClicked;
        public event Action OnResumeGameButtonIsClicked;
        
        public IGameScreenPauseVIew View { get; }
        
        public IGameScreenPresenter _gameScreenPresenter;

        public bool _isInit = false;

        [Inject] public GameScreenPausePresenter(IGameScreenPauseVIew view)
        {
            View = view;
            
            Init();
        }

        public void Show() => View.Show();
        public void Hide(System.Action callBack = null) => View.Hide();
        
        public void Init()
        {
            if (_isInit)
                return;

            View.InitPresentor(this);
            _isInit = true;
        }


        public void OnMainMenuButtonClicked()
        {
            // снять паузу и продолжыть игру
            OnMainMenuButtonIsClicked?.Invoke();
            View.Hide();
        }

        public void OnResumeGameButtondClicked()
        {
            // начать игру заново
            OnResumeGameButtonIsClicked?.Invoke();
            View.Hide(); 
        }
    }
}