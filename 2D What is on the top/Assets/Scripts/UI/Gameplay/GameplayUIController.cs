using System;
using System.Collections;
using System.Collections.Generic;
using Obstacles;
using PersistentData;
using ResourcesCollector;
using Systems.SceneSystem;
using UI.GameScreenPause;
using UI.MVP;
using UnityEngine;
using Zenject;

namespace UI
{
    public class GameplayUIController : MonoBehaviour
    {
        private const float ShowDefeatScreenDelay = 1.5f;
        
        private List<IPresenter> _presenters = new();
        
        private IGameScreenPresenter _gameScreenHUDPresenter;
        private IGameScreenDefeatPresenter _gameScreenDefeatPresenter;
        private IGameScreenPausePresenter _gameScreenPausePresenter;

        private IResourceCollector _resourceCollector;
        private SceneController sceneController;

        private IPersistentPlayerData _persistentPlayerData;
        private LevelsDB _levelsDB;
                
        [Inject] private void Construct(
            IResourceCollector resourceCollector,
            IGameScreenPresenter gameScreenPresenter,
            IGameScreenDefeatPresenter gameScreenDefeatPresenter,
            IGameScreenPausePresenter gameScreenPausePresenter,
            IPersistentPlayerData persistentPlayerData,
            LevelsDB levelsDB,
            SceneController sceneController
            )
        { 
            _gameScreenHUDPresenter = gameScreenPresenter;
            _gameScreenDefeatPresenter = gameScreenDefeatPresenter;
            _gameScreenPausePresenter = gameScreenPausePresenter;
            
            _resourceCollector = resourceCollector;
            this.sceneController = sceneController;

            _persistentPlayerData = persistentPlayerData;
            _levelsDB = levelsDB;
        }

        private void Awake()
        {
            _presenters.Add(_gameScreenHUDPresenter);
            _presenters.Add(_gameScreenDefeatPresenter);
            _presenters.Add(_gameScreenPausePresenter);
        }

        private void OnEnable()
        {
            _gameScreenDefeatPresenter.HomeButtonCliked += OnClaimRewardAndGoToMainMenuButtonClickedFromLosePanel;
            _gameScreenDefeatPresenter.OnX2RewardButtonCliked += OnX2RewardButtonClickedFromLosePanel;
            
            
            _gameScreenHUDPresenter.OnPauseClicked += OnPauseGame;
            
            _gameScreenPausePresenter.OnResumeGameButtonIsClicked += OnResumeGameButtonIs;
            _gameScreenPausePresenter.OnMainMenuButtonIsClicked += OnMainMenuButtonClicked;
            _resourceCollector.ResourcesContainerChange += OnResourcesContainerChanged;
            

            EventAggregator.Subscribe<SwitchGameStateToPlayGameEvent>(OnSwitchGameStateToPlay);
            EventAggregator.Subscribe<SwitchGameStateToLoseGameEvent>(OnSwitchGameStateToLoseGame);
            EventAggregator.Subscribe<SwitchGameStateToWinGameEvent>(OnSwitchGameToMainMenu);
        }

        private void OnDisable()
        {
            _gameScreenDefeatPresenter.HomeButtonCliked -= OnClaimRewardAndGoToMainMenuButtonClickedFromLosePanel;
            _gameScreenDefeatPresenter.OnX2RewardButtonCliked -= OnX2RewardButtonClickedFromLosePanel;
            
            _gameScreenHUDPresenter.OnPauseClicked -= OnPauseGame;
            
            _gameScreenPausePresenter.OnResumeGameButtonIsClicked -= OnResumeGameButtonIs;
            _gameScreenPausePresenter.OnMainMenuButtonIsClicked -= OnMainMenuButtonClicked;
            
            _resourceCollector.ResourcesContainerChange -= OnResourcesContainerChanged;
            

            EventAggregator.Unsubscribe<SwitchGameStateToPlayGameEvent>(OnSwitchGameStateToPlay);
            EventAggregator.Unsubscribe<SwitchGameStateToLoseGameEvent>(OnSwitchGameStateToLoseGame);
            EventAggregator.Unsubscribe<SwitchGameStateToWinGameEvent>(OnSwitchGameToMainMenu);
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
        
        private void HideAllViewsInList()
        {
            foreach (var presenter in _presenters)
                presenter.Hide();
        }
        
        private void OnSwitchGameToMainMenu(object sender, SwitchGameStateToWinGameEvent eventData)
        {
            _gameScreenHUDPresenter.Hide(() =>
            {
                EventAggregator.Post(this, new SwitchCameraStateOnPlayerIsNotOnThePlatform());
            });
        }
        
        private void OnSwitchGameStateToPlay(object sender, SwitchGameStateToPlayGameEvent evenData)
        {
            _gameScreenHUDPresenter.Init();
            HideOtherViewsAndShow(_gameScreenHUDPresenter);
        }


        private void OnSwitchGameStateToLoseGame(object sender, SwitchGameStateToLoseGameEvent evenData)
        {
            EventAggregator.Post(this, new SwitchCameraStateOnPlayerLoseIsNotOnPlatform());
            StartCoroutine(DefeatCoroutineDelay());
        }

        private void OnResumeGameButtonIs()
        {
            _gameScreenPausePresenter.Hide(() =>
            {
                EventAggregator.Post(_gameScreenPausePresenter, new ResumeGameEventHandler());
            });
        }
        
        private void OnPauseGame()
        {
            // EventAggregator.Post(_gameScreenPausePresenter, new PauseGameEventHandler());
            _gameScreenPausePresenter.Show();
        }

        private void OnMainMenuButtonClicked()
        {
            sceneController.RestartCurrentScent();                                               
            _persistentPlayerData.PlayerData.SetCurrentLevel(_levelsDB.CurrentLevel);
            EventAggregator.Post(_gameScreenPausePresenter, new ResumeGameEventHandler());
            EventAggregator.Post(_gameScreenHUDPresenter, new SwitchCameraStateOnMainMenuPlatform());
            EventAggregator.Post(_gameScreenHUDPresenter, new SwitchGameStateToMainMenuGameEvent());
        }

        private void OnX2RewardButtonClickedFromLosePanel()
        {
            
            // проверка просмотра рекламы ...
            // если игрок посмотрел дать x2 награду и снять с паузы
            // если человек не досмотрел до конца вывести окно что он не досмотрел рекламу и просто зачислить обычнуб x1 награду и снять с паузы 
            
            _gameScreenDefeatPresenter.Hide(() =>
            {
                Debug.Log("X2 Reward Button Clicked");
                EventAggregator.Post(this, new ClaimRewardEvent());
                HideAllViewsInList();
            });
        }

        private void OnClaimRewardAndGoToMainMenuButtonClickedFromLosePanel()
        {
            _gameScreenDefeatPresenter.Hide(() =>
            {
                Debug.Log("Claim Reward");
                EventAggregator.Post(this, new ClaimRewardEvent());
                HideAllViewsInList();
            });
        }
        
        private void OnResourcesContainerChanged(Dictionary<ResourceTypes, int> data)
        {
            foreach (var (key, value) in data) // # todo - поменять бы и сделать нормально а то это не дела P.S можно подсмотреть в MainMenuScreenPresenter
            {
                switch (key)
                {
                    case ResourceTypes.Coin:
                        _gameScreenHUDPresenter.SetAmountCoins(value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        
        private IEnumerator DefeatCoroutineDelay()
        {
            yield return new WaitForSeconds(ShowDefeatScreenDelay);
            HideOtherViewsAndShow(_gameScreenDefeatPresenter);
        }
    }
}