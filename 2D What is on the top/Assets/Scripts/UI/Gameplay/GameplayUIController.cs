using System.Collections.Generic;
using UI.GameScreenPause;
using UI.MVP;
using UnityEngine;
using Zenject;

namespace UI
{
    public class GameplayUIController : MonoBehaviour
    {

        private List<IPresenter> _presenters = new();
        
        private IGameScreenPresenter _gameScreenPresenter;
        private IGameScreenDefeatPresenter _gameScreenDefeatPresenter;
        private IGameScreenPausePresenter _gameScreenPausePresenter;
        
        private ICharacterEvents characterEvents;
        
        [Inject] private void Construct(
            ICharacterEvents characterEvents,
            IGameScreenPresenter gameScreenPresenter,
            IGameScreenDefeatPresenter gameScreenDefeatPresenter,
            IGameScreenPausePresenter gameScreenPausePresenter
            )
        {
            this.characterEvents = characterEvents;
            
            _gameScreenPresenter = gameScreenPresenter;
            _gameScreenDefeatPresenter = gameScreenDefeatPresenter;
            _gameScreenPausePresenter = gameScreenPausePresenter;
            
            Debug.Log($"is {gameScreenPresenter}");
            Debug.Log($"is {gameScreenDefeatPresenter}");
            Debug.Log($"is {gameScreenPausePresenter}");
            
            _presenters.Add(_gameScreenPresenter);
            _presenters.Add(_gameScreenDefeatPresenter);
            _presenters.Add(_gameScreenPausePresenter);
        }

        private void OnEnable()
        {
            characterEvents.CharacterDefeat += OnDefaet;
            _gameScreenPresenter.OnPauseClicked += OnPauseGame;
            _gameScreenPausePresenter.OnResumeGameClicked += OnResumeGame;
            _gameScreenPausePresenter.OnRestartGameClicked += OnRestartGame;
        }

        private void OnDisable()
        {
            characterEvents.CharacterDefeat -= OnDefaet;
            _gameScreenPresenter.OnPauseClicked -= OnPauseGame;
            _gameScreenPausePresenter.OnResumeGameClicked -= OnResumeGame;
            _gameScreenPausePresenter.OnRestartGameClicked -= OnRestartGame;
        }

        private void HideOtherViewsAndShow(IPresenter presenter)
        {
            foreach (var presntr in _presenters)
            {
                if (presenter != presntr)
                    presntr.Hide();
            }
            
            presenter.Show();
        }

        private void OnDefaet() => HideOtherViewsAndShow(_gameScreenDefeatPresenter);

        private void OnRestartGame() => Debug.Log("Restart game logic");
        
        private void OnResumeGame() => Time.timeScale = 1;

        private void OnPauseGame() => Time.timeScale = 0;


    }
}