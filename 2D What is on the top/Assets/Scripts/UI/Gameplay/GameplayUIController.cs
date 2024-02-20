using System;
using System.Collections.Generic;
using Game.Gameplay;
using ResourcesCollector;
using UI.GameScreenLevelWinn;
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
        private IGameScreenLevelWinPresenter _gameScreenLevelWinPresenter;

        private IResourceCollector _resourceCollector;
        // private IGameplay _gameplay;
        
        [Inject] private void Construct(
            // IGameplay gameplay,
            IResourceCollector resourceCollector,
            
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

            _resourceCollector = resourceCollector;
            // _gameplay = gameplay;
        }
        
        private void OnEnable()
        {
            // _gameplay.PlayerWin += OnPlayerWin;
            // _gameplay.PlayerDefeat += OnPlayerDefeat;
            _gameScreenPresenter.OnPauseClicked += OnPauseGame;
            _gameScreenPausePresenter.OnResumeGameClicked += OnResumeGame;
            _gameScreenPausePresenter.OnRestartGameClicked += OnRestartGame;

            _resourceCollector.ResourcesContainerChange += OnResourcesContainerChanged;
            
            
            EventAggregator.Subscribe<PlayerWinEventHandler>(OnPlayerWin);
            EventAggregator.Subscribe<PlayerLoseEventHandler>(OnPlayerLose);
        }

        private void OnDisable()
        {
            // _gameplay.PlayerWin -= OnPlayerWin;
            // _gameplay.PlayerDefeat -= OnPlayerDefeat;
            _gameScreenPresenter.OnPauseClicked -= OnPauseGame;
            _gameScreenPausePresenter.OnResumeGameClicked -= OnResumeGame;
            _gameScreenPausePresenter.OnRestartGameClicked -= OnRestartGame;
            
            _resourceCollector.ResourcesContainerChange -= OnResourcesContainerChanged;
            
            
            EventAggregator.Unsubscribe<PlayerWinEventHandler>(OnPlayerWin);
            EventAggregator.Unsubscribe<PlayerLoseEventHandler>(OnPlayerLose);
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

        // private void OnPlayerDefeat() => HideOtherViewsAndShow(_gameScreenDefeatPresenter);
        // private void OnPlayerWin() => HideOtherViewsAndShow(_gameScreenLevelWinPresenter);

        private void OnPlayerLose(object arg1, PlayerLoseEventHandler arg2) =>
            HideOtherViewsAndShow(_gameScreenDefeatPresenter);
        private void OnPlayerWin(object sender, PlayerWinEventHandler eventHandler) =>
            HideOtherViewsAndShow(_gameScreenLevelWinPresenter);
        
        
        private void OnRestartGame() => Debug.Log("Restart game logic");

        private void OnResumeGame()
        {
            // _gameplay.ResumeGame();
            EventAggregator.Post(_gameScreenPausePresenter, new ResumeGameEventHandler());
            _gameScreenPausePresenter.Hide();
        }
        
        private void OnPauseGame()
        {
            // _gameplay.PauseGame();
            EventAggregator.Post(_gameScreenPausePresenter, new PauseGameEventHandler());
            _gameScreenPausePresenter.Show();
        }

        private void OnResourcesContainerChanged(Dictionary<ResourceStorageTypes, int> data)
        {
            foreach (var (key, value) in data)
            {
                switch (key)
                {
                    case ResourceStorageTypes.Coin:
                        _gameScreenPresenter.SetAmountCoins(value);
                        break;
                    case ResourceStorageTypes.AdhesivePlaster:
                        break;
                    case ResourceStorageTypes.CrumpledPlasticBootle:
                        break;
                    case ResourceStorageTypes.CrumpledSodaCan:
                        break;
                    case ResourceStorageTypes.Glasses:
                        break;
                    case ResourceStorageTypes.PieceOfFabricsGreen:
                        break;
                    case ResourceStorageTypes.PieceOfFabricsYellow:
                        break;
                    case ResourceStorageTypes.PieceOfFabricsOrange:
                        break;
                    case ResourceStorageTypes.PlasticlBagYellow:
                        break;
                    case ResourceStorageTypes.PlasticlBagBlue:
                        break;
                    case ResourceStorageTypes.PlasticlBagGreen:
                        break;
                    case ResourceStorageTypes.PlasticlBagRed:
                        break;
                    case ResourceStorageTypes.PlasticlBagPurple:
                        break;
                    case ResourceStorageTypes.PlateSimple:
                        break;
                    case ResourceStorageTypes.PlateRed:
                        break;
                    case ResourceStorageTypes.PlateFlowers:
                        break;
                    case ResourceStorageTypes.PlateMixColored:
                        break;
                    case ResourceStorageTypes.Spoon:
                        break;
                    case ResourceStorageTypes.ToothBrushRed:
                        break;
                    case ResourceStorageTypes.ToothBrushWhithe:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}