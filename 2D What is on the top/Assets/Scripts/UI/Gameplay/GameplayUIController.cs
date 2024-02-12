using System;
using System.Collections.Generic;
using Game.Gameplay;
using UI.GameScreenLevelWinn;
using UI.GameScreenPause;
using UI.MVP;
using UnityEngine;
using Zenject;

namespace UI
{
    public class GameplayUIController : MonoBehaviour
    {
        [SerializeField] private GameplayController _gameplay;
        
        private List<IPresenter> _presenters = new();
        
        private IGameScreenPresenter _gameScreenPresenter;
        private IGameScreenDefeatPresenter _gameScreenDefeatPresenter;
        private IGameScreenPausePresenter _gameScreenPausePresenter;
        private IGameScreenLevelWinPresenter _gameScreenLevelWinPresenter;
        
        
        [Inject] private void Construct(
            ICharacterEvents characterEvents,
            IGameScreenPresenter gameScreenPresenter,
            IGameScreenDefeatPresenter gameScreenDefeatPresenter,
            IGameScreenPausePresenter gameScreenPausePresenter,
            IGameScreenLevelWinPresenter gameScreenLevelWinPresenter
            )
        { 
            _gameScreenPresenter = gameScreenPresenter;
            _gameScreenDefeatPresenter = gameScreenDefeatPresenter;
            _gameScreenPausePresenter = gameScreenPausePresenter;
            _gameScreenLevelWinPresenter = gameScreenLevelWinPresenter;
            
            _presenters.Add(_gameScreenPresenter);
            _presenters.Add(_gameScreenDefeatPresenter);
            _presenters.Add(_gameScreenPausePresenter);
            _presenters.Add(_gameScreenLevelWinPresenter);
        }
        
        private void OnEnable()
        {
            _gameplay.PlayerWin += OnPlayerWin;
            _gameplay.PlayerDefeat += OnPlayerDefeat;
            _gameScreenPresenter.OnPauseClicked += OnPauseGame;
            _gameScreenPausePresenter.OnResumeGameClicked += OnResumeGame;
            _gameScreenPausePresenter.OnRestartGameClicked += OnRestartGame;
        }


        private void OnDisable()
        {
            _gameplay.PlayerWin -= OnPlayerWin;
            _gameplay.PlayerDefeat -= OnPlayerDefeat;
            _gameScreenPresenter.OnPauseClicked -= OnPauseGame;
            _gameScreenPausePresenter.OnResumeGameClicked -= OnResumeGame;
            _gameScreenPausePresenter.OnRestartGameClicked -= OnRestartGame;
        }

        public void HideOtherViewsAndShow(IPresenter presenter)
        {
            foreach (var presntr in _presenters)
            {
                if (presenter != presntr)
                    presntr.Hide();
            }
            
            presenter.Show();
        }

        private void OnPlayerDefeat() => HideOtherViewsAndShow(_gameScreenDefeatPresenter);

        private void OnPlayerWin() => HideOtherViewsAndShow(_gameScreenLevelWinPresenter);
        
        private void OnRestartGame() => Debug.Log("Restart game logic");

        private void OnResumeGame() => _gameplay.ResumeGame();

        private void OnPauseGame() => _gameplay.PauseGame();
    }
}