using System;
using System.Collections.Generic;
using ResourcesCollector;
using Systems.SceneSystem;
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
        
        private IGameScreenPresenter _gameScreenHUDPresenter;
        private IGameScreenDefeatPresenter _gameScreenDefeatPresenter;
        private IGameScreenPausePresenter _gameScreenPausePresenter;
        private IGameScreenLevelWinPresenter _gameScreenLevelWinPresenter;

        private IResourceCollector _resourceCollector;
        private ISceneSystem _sceneSystem;
        
        [Inject] private void Construct(
            ISceneSystem sceneSystem,
            IResourceCollector resourceCollector,
            
            IGameScreenPresenter gameScreenPresenter,
            IGameScreenDefeatPresenter gameScreenDefeatPresenter,
            IGameScreenPausePresenter gameScreenPausePresenter,
            IGameScreenLevelWinPresenter gameScreenLevelWinPresenter
            )
        { 
            _gameScreenHUDPresenter = gameScreenPresenter;
            _gameScreenDefeatPresenter = gameScreenDefeatPresenter;
            _gameScreenPausePresenter = gameScreenPausePresenter;
            _gameScreenLevelWinPresenter = gameScreenLevelWinPresenter;
            
            _resourceCollector = resourceCollector;
            _sceneSystem = sceneSystem;
        }

        private void Awake()
        {
            _presenters.Add(_gameScreenHUDPresenter);
            _presenters.Add(_gameScreenDefeatPresenter);
            _presenters.Add(_gameScreenPausePresenter);
            _presenters.Add(_gameScreenLevelWinPresenter);
        }

        private void OnEnable()
        {
            _gameScreenDefeatPresenter.HomeButtonCliked += OnHomeButtonClicked;
            _gameScreenDefeatPresenter.OnX2RewardButtonCliked += OnX2RewardButtonClicked;
            _gameScreenDefeatPresenter.RestartLevelButtonCliked += OnRestartGame;

            _gameScreenLevelWinPresenter.X2RewardButtonClicked += OnX2RewardButtonClicked;
            _gameScreenLevelWinPresenter.ClaimButtonClicked += OnHomeButtonClicked;
            
            _gameScreenHUDPresenter.OnPauseClicked += OnPauseGame;
            
            _gameScreenPausePresenter.OnResumeGameClicked += OnResumeGame;
            _gameScreenPausePresenter.OnRestartGameClicked += OnRestartGame;

            _resourceCollector.ResourcesContainerChange += OnResourcesContainerChanged;
            
            
            EventAggregator.Subscribe<PlayerWinEventHandler>(OnPlayerWin);
            EventAggregator.Subscribe<PlayerLoseEventHandler>(OnPlayerLose);
        }

        private void OnDisable()
        {
            _gameScreenDefeatPresenter.HomeButtonCliked -= OnHomeButtonClicked;
            _gameScreenDefeatPresenter.OnX2RewardButtonCliked -= OnX2RewardButtonClicked;
            _gameScreenDefeatPresenter.RestartLevelButtonCliked -= OnRestartGame;
            
            _gameScreenHUDPresenter.OnPauseClicked -= OnPauseGame;
            
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

        private void OnPlayerLose(object sender, PlayerLoseEventHandler eventHandler) => HideOtherViewsAndShow(_gameScreenDefeatPresenter);
        
        private void OnPlayerWin(object sender, PlayerWinEventHandler eventHandler)
        {
            // # todo - Должен пофиксить чтобы когда игрок выбрал и он падал вниз то не реагировал на перепятствия потому что сеейчас игрок выйграл ему показывают выйграшное окно и когда он падает и задевает препятсвия то ему показывет окно проиграша
            HideOtherViewsAndShow(_gameScreenLevelWinPresenter);
        }

        private void OnRestartGame()
        {
            HideOtherViewsAndShow(_gameScreenHUDPresenter);
            OnResumeGame();
            _sceneSystem.ReloadScene();
            // _sceneLoader.RestatcCurrentLevel();
        }

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

        private void OnX2RewardButtonClicked() => Debug.Log("X2 Reward Button Clicked");
        
        private void OnHomeButtonClicked()
        {
            // _sceneLoader.GoToMainMenu();
        }

        private void OnResourcesContainerChanged(Dictionary<ResourceTypes, int> data)
        {
            foreach (var (key, value) in data)
            {
                switch (key)
                {
                    case ResourceTypes.Coin:
                        _gameScreenHUDPresenter.SetAmountCoins(value);
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