using System;
using UI.GameScreenPause;
using UI.MVP;
using UnityEngine;
using Zenject;

namespace UI
{
    public interface IGameScreenPresenter : IPresenter<IGameScreenModel, IGameSreenView>
    {
        public event Action OnPauseClicked; 
        void OnPauseButtonClicked();
    }

    public class GameScreenPresenter : IGameScreenPresenter, IDisposable
    {
        public event Action OnPauseClicked;
        
        public IGameSreenView View { get; private set; }
        public IGameScreenModel Model { get; private set; }
        
        private CharacterData _characterData;
        private IGameScreenPausePresenter _gameScreenPausePresenter;
        
        private bool _isInit = false;

        [Inject] public GameScreenPresenter(IGameScreenModel model, IGameSreenView view, CharacterData characterData, IGameScreenPausePresenter gameScreenPausePresenter)
        {
            Model = model;
            View = view;
            _characterData = characterData;

            _gameScreenPausePresenter = gameScreenPausePresenter;
            
            Init();
        }

        public void Init()
        {
            if (_isInit)
                return;

            _isInit = true;

            View.Initialize(_characterData.StaminaData);
            View.InitPresentor(this);
            
            Model.HightScoreChange += OnHightScoreChange;
            Model.StaminaChange += OnStaminaChange;

            _gameScreenPausePresenter.OnRestartGameClicked += OnRestartGame;
            _gameScreenPausePresenter.OnResumeGameClicked += OnResumeGame;
        }

        private void OnResumeGame()
        {
            _gameScreenPausePresenter.Hide();
            Debug.Log("need To paused game");
            View.Show();
        }

        private void OnRestartGame()
        {
            Debug.Log("need to realize - restart game !!");
            _gameScreenPausePresenter.Hide();
            View.Show();
        }

        public void Dispose()
        {
            Model.HightScoreChange -= OnHightScoreChange;
            Model.StaminaChange -= OnStaminaChange;
            _gameScreenPausePresenter.OnRestartGameClicked -= OnRestartGame;
            _gameScreenPausePresenter.OnResumeGameClicked -= OnResumeGame;
        }
        
        public void Show() { PrepareData(); View.Show(); }
        public void Hide() => View.Hide();

        public void UpdateHeightScore(int score) => Model.Score = score;

        public void UpdateStamina(float stamina) => Model.Stamina = stamina;

        private void PrepareData()
        {
            View.SetHightScore(Model.Score);
            View.SetStaminaValue(Model.Stamina);
        }

        public void OnPauseButtonClicked()
        {
            OnPauseClicked?.Invoke(); 
            _gameScreenPausePresenter.Show();
        }

        private void OnStaminaChange(float stamina)
        {
            View.SetStaminaValue(stamina);
        }
        
        private void OnHightScoreChange(int score)
        {
            View.SetHightScore(score);
        }
    }
}