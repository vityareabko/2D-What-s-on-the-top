using System;
using System.Collections;
using System.Collections.Generic;
using ResourcesCollector;
using UI.GameScreenLevelWinn;
using UI.GameScreenPause;
using UI.MVP;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            _gameScreenHUDPresenter = gameScreenPresenter;
            _gameScreenDefeatPresenter = gameScreenDefeatPresenter;
            _gameScreenPausePresenter = gameScreenPausePresenter;
            _gameScreenLevelWinPresenter = gameScreenLevelWinPresenter;
            
            _resourceCollector = resourceCollector;
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

            _gameScreenLevelWinPresenter.X2RewardButtonClicked += OnX2RewardButtonClicked;
            _gameScreenLevelWinPresenter.ClaimButtonClicked += OnClaimRewardAndGoToMainMenuButtonClicked;
            
            _gameScreenHUDPresenter.OnPauseClicked += OnPauseGame;
            
            _gameScreenPausePresenter.OnResumeGameButtonIsClicked += OnResumeGameButtonIs;
            _gameScreenPausePresenter.OnMainMenuButtonIsClicked += OnMainMenuButtonClicked;
            _resourceCollector.ResourcesContainerChange += OnResourcesContainerChanged;
            

            EventAggregator.Subscribe<SwitchGameStateToPlayGameEvent>(OnSwitchGameStateToPlay);
            EventAggregator.Subscribe<SwitchGameStateToLoseGameEvent>(OnSwitchGameStateToLoseGame);
        }

        private void OnDisable()
        {
            _gameScreenDefeatPresenter.HomeButtonCliked -= OnClaimRewardAndGoToMainMenuButtonClicked;
            _gameScreenDefeatPresenter.OnX2RewardButtonCliked -= OnX2RewardButtonClicked;
            
            _gameScreenHUDPresenter.OnPauseClicked -= OnPauseGame;
            
            _gameScreenPausePresenter.OnResumeGameButtonIsClicked -= OnResumeGameButtonIs;
            _gameScreenPausePresenter.OnMainMenuButtonIsClicked -= OnMainMenuButtonClicked;
            
            _resourceCollector.ResourcesContainerChange -= OnResourcesContainerChanged;
            

            EventAggregator.Unsubscribe<SwitchGameStateToPlayGameEvent>(OnSwitchGameStateToPlay);
            EventAggregator.Unsubscribe<SwitchGameStateToLoseGameEvent>(OnSwitchGameStateToLoseGame);
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
            _gameScreenPausePresenter.Hide(() =>
            {
            
                EventAggregator.Post(_gameScreenPausePresenter, new ResumeGameEventHandler());
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                EventAggregator.Post(_gameScreenHUDPresenter, new SwitchCameraStateOnMainMenuPlatform());
                EventAggregator.Post(_gameScreenHUDPresenter, new SwitchGameStateToMainMenuGameEvent());
                
            });
        }

        private void OnX2RewardButtonClicked()
        {
            
            // проверка просмотра рекламы ...
            // если игрок посмотрел дать x2 награду и снять с паузы
            // если человек не досмотрел до конца вывести окно что он не досмотрел рекламу и просто зачислить обычнуб x1 награду и снять с паузы 
            
            Debug.Log("X2 Reward Button Clicked");
            
            EventAggregator.Post(this, new ClaimRewardEvent());
            
            HideAllViewsInList();
        }

        private void OnClaimRewardAndGoToMainMenuButtonClicked()
        {
            Debug.Log("Claim Reward");
            
            EventAggregator.Post(this, new ClaimRewardEvent());

            HideAllViewsInList();
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
        
        
        private IEnumerator DefeatCoroutineDelay()
        {
            yield return new WaitForSeconds(ShowDefeatScreenDelay);
            HideOtherViewsAndShow(_gameScreenDefeatPresenter);
        }
    }
}