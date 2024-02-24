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
        
        [Inject] private void Construct(
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
            

            _resourceCollector = resourceCollector;
        }

        private void Awake()
        {
            _presenters.Add(_gameScreenPresenter);
            _presenters.Add(_gameScreenDefeatPresenter);
            _presenters.Add(_gameScreenPausePresenter);
            _presenters.Add(_gameScreenLevelWinPresenter);
        }

        private void OnEnable()
        {
            _gameScreenPresenter.OnPauseClicked += OnPauseGame;
            _gameScreenPausePresenter.OnResumeGameClicked += OnResumeGame;
            _gameScreenPausePresenter.OnRestartGameClicked += OnRestartGame;

            _resourceCollector.ResourcesContainerChange += OnResourcesContainerChanged;
            
            
            EventAggregator.Subscribe<PlayerWinEventHandler>(OnPlayerWin);
            EventAggregator.Subscribe<PlayerLoseEventHandler>(OnPlayerLose);
        }

        private void OnDisable()
        {
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

        private void OnPlayerLose(object arg1, PlayerLoseEventHandler arg2) =>
            HideOtherViewsAndShow(_gameScreenDefeatPresenter);
        private void OnPlayerWin(object sender, PlayerWinEventHandler eventHandler) =>
            HideOtherViewsAndShow(_gameScreenLevelWinPresenter);
        
        private void OnRestartGame() => Debug.Log("Restart game logic");

        private void OnResumeGame()
        {
            EventAggregator.Post(_gameScreenPausePresenter, new ResumeGameEventHandler());
            _gameScreenPausePresenter.Hide();
        }
        
        private void OnPauseGame()
        {
            EventAggregator.Post(_gameScreenPausePresenter, new PauseGameEventHandler());
            _gameScreenPausePresenter.Show();
        }

        private void OnResourcesContainerChanged(Dictionary<ResourceTypes, int> data)
        {
            foreach (var (key, value) in data)
            {
                switch (key)
                {
                    case ResourceTypes.Coin:
                        _gameScreenPresenter.SetAmountCoins(value);
                        break;
                    case ResourceTypes.AdhesivePlaster:
                        break;
                    case ResourceTypes.CrumpledPlasticBootle:
                        break;
                    case ResourceTypes.CrumpledSodaCan:
                        break;
                    case ResourceTypes.Glasses:
                        break;
                    case ResourceTypes.PieceOfFabricsGreen:
                        break;
                    case ResourceTypes.PieceOfFabricsYellow:
                        break;
                    case ResourceTypes.PieceOfFabricsOrange:
                        break;
                    case ResourceTypes.PlasticlBagYellow:
                        break;
                    case ResourceTypes.PlasticlBagBlue:
                        break;
                    case ResourceTypes.PlasticlBagGreen:
                        break;
                    case ResourceTypes.PlasticlBagRed:
                        break;
                    case ResourceTypes.PlasticlBagPurple:
                        break;
                    case ResourceTypes.PlateSimple:
                        break;
                    case ResourceTypes.PlateRed:
                        break;
                    case ResourceTypes.PlateFlowers:
                        break;
                    case ResourceTypes.PlateMixColored:
                        break;
                    case ResourceTypes.Spoon:
                        break;
                    case ResourceTypes.ToothBrushRed:
                        break;
                    case ResourceTypes.ToothBrushWhithe:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}