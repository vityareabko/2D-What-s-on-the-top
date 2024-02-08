using UI.MVP;
using UnityEngine;

namespace UI
{
    public interface IGameScreenPresenter : IPresenter<IGameScreenModel, IGameSreenView>
    {
        
    }

    public class GameScreenPresenter : IGameScreenPresenter
    {
        public IGameSreenView View { get; }
        public IGameScreenModel Model { get; }

        private bool _isInit = false;

        public GameScreenPresenter(IGameScreenModel model, IGameSreenView view)
        {
            Model = model;
            View = view;
            
            Model.ScoreChange += OnScoreChange;
            View.InitPresentor(this);
        }
        
        public void Init()
        {
            if (_isInit) 
                return;
            
            _isInit = true;
            OnScoreChange(Model.Score);
        }
        
        public void Show() => View.Show(); 
        public void Hide() => View.Hide();

        private void OnScoreChange(int score)
        {
            View.SetScore(score);
        }
    }
}