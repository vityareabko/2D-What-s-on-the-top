using System;
using System.Collections.Generic;
using GameSM;
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
            // ISceneSystem sceneSystem,
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
            // _sceneSystem = sceneSystem;
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
            _gameScreenDefeatPresenter.HomeButtonCliked += OnClaimRewardAndGoToMainMenuButtonClicked;
            _gameScreenDefeatPresenter.OnX2RewardButtonCliked += OnX2RewardButtonClicked;
            _gameScreenDefeatPresenter.RestartLevelButtonCliked += OnRestartGame;

            _gameScreenLevelWinPresenter.X2RewardButtonClicked += OnX2RewardButtonClicked;
            _gameScreenLevelWinPresenter.ClaimButtonClicked += OnClaimRewardAndGoToMainMenuButtonClicked;
            
            _gameScreenHUDPresenter.OnPauseClicked += OnPauseGame;
            
            _gameScreenPausePresenter.OnResumeGameClicked += OnResumeGame;
            _gameScreenPausePresenter.OnRestartGameClicked += OnRestartGame;

            _resourceCollector.ResourcesContainerChange += OnResourcesContainerChanged;
            
            
            // EventAggregator.Subscribe<PlayerLoseEventHandler>(OnPlayerLose);
            

            EventAggregator.Subscribe<SwitchGameStateToPlayGameEvent>(OnSwitchGameStateToPlay);
            EventAggregator.Subscribe<SwitchGameStateToLoseGameEvent>(OnSwitchGameStateToLoseGame);
        }

        private void OnDisable()
        {
            _gameScreenDefeatPresenter.HomeButtonCliked -= OnClaimRewardAndGoToMainMenuButtonClicked;
            _gameScreenDefeatPresenter.OnX2RewardButtonCliked -= OnX2RewardButtonClicked;
            _gameScreenDefeatPresenter.RestartLevelButtonCliked -= OnRestartGame;
            
            _gameScreenHUDPresenter.OnPauseClicked -= OnPauseGame;
            
            _gameScreenPausePresenter.OnResumeGameClicked -= OnResumeGame;
            _gameScreenPausePresenter.OnRestartGameClicked -= OnRestartGame;
            
            _resourceCollector.ResourcesContainerChange -= OnResourcesContainerChanged;
            
            // EventAggregator.Unsubscribe<PlayerLoseEventHandler>(OnPlayerLose);

            EventAggregator.Unsubscribe<SwitchGameStateToPlayGameEvent>(OnSwitchGameStateToPlay);
            EventAggregator.Unsubscribe<SwitchGameStateToLoseGameEvent>(OnSwitchGameStateToLoseGame);
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
        
        private void OnSwitchGameStateToPlay(object sender, SwitchGameStateToPlayGameEvent evenData) =>
            HideOtherViewsAndShow(_gameScreenHUDPresenter);
        

        private void OnSwitchGameStateToLoseGame(object sender, SwitchGameStateToLoseGameEvent evenData) =>
            HideOtherViewsAndShow(_gameScreenDefeatPresenter); 
        
        
        // private void OnPlayerLose(object sender, PlayerLoseEventHandler eventHandler)
        // {
        //     HideOtherViewsAndShow(_gameScreenDefeatPresenter);
        //     EventAggregator.Post(_gameScreenHUDPresenter, new SwitchGameStateToMainMenuGameEvent());
        // }

        private void OnRestartGame()
        {
            HideOtherViewsAndShow(_gameScreenHUDPresenter);
            OnResumeGame();
            
            // _sceneSystem.ReloadSceneByStateGame(GameStateType.GamePlay);
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

        private void OnX2RewardButtonClicked()
        {
            Debug.Log("X2 Reward Button Clicked");
            EventAggregator.Post(_gameScreenHUDPresenter, new SwitchGameStateToMainMenuGameEvent());
        }

        private void OnClaimRewardAndGoToMainMenuButtonClicked()
        {
            Debug.Log("Claim Reward");
            EventAggregator.Post(_gameScreenHUDPresenter, new SwitchGameStateToMainMenuGameEvent());
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