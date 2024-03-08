using System.Collections.Generic;
using UI.MainMenu.ShopSkinsScreen;
using UI.MVP;
using UnityEngine;
using VHierarchy.Libs;
using Zenject;

namespace UI.MainMenu
{
    public class MainMenuUIController : MonoBehaviour
    {
        private IMainMenuPresenter _mainMenuPresenter;
        private IShopSkinsScreenPresenter _shopSkinsPresenter;

        private List<IPresenter> _presenters = new();
        
        [Inject] private void Construct(IMainMenuPresenter mainMenuPresenter, IShopSkinsScreenPresenter shopSkinsPresenter)
        {
            _mainMenuPresenter = mainMenuPresenter;
            _shopSkinsPresenter = shopSkinsPresenter;
            
            _presenters.Add(_mainMenuPresenter);
            _presenters.Add(_shopSkinsPresenter);
        }

        private void OnEnable()
        {
            _mainMenuPresenter.ClickedPlayButton += OnClickedPlatButton;
            _mainMenuPresenter.ClickedShopSkinsButton += OnClickedShopSkinsButton;

            _shopSkinsPresenter.ClickBackButton += OnClickedShopSkinsBackButton;
            
            EventAggregator.Subscribe<SwitchGameStateToPlayGameEvent>(OnSwitchToGameStateToPlay);
            EventAggregator.Subscribe<SwitchGameStateToMainMenuGameEvent>(OnSwitchToGameStateMainMenu);
        }


        private void OnDisable()
        {
            _mainMenuPresenter.ClickedPlayButton -= OnClickedPlatButton;
            _mainMenuPresenter.ClickedShopSkinsButton -= OnClickedShopSkinsButton;
            
            _shopSkinsPresenter.ClickBackButton -= OnClickedShopSkinsBackButton;
            
            EventAggregator.Unsubscribe<SwitchGameStateToPlayGameEvent>(OnSwitchToGameStateToPlay);
            EventAggregator.Unsubscribe<SwitchGameStateToMainMenuGameEvent>(OnSwitchToGameStateMainMenu);
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

        private void OnClickedShopSkinsBackButton()
        {
            HideOtherViewsAndShow(_mainMenuPresenter);
            EventAggregator.Post(this, new SwitchCameraStateOnMainMenuPlatform());
        }

        private void OnSwitchToGameStateMainMenu(object sender, SwitchGameStateToMainMenuGameEvent eventData)
        {
            HideOtherViewsAndShow(_mainMenuPresenter);
            EventAggregator.Post(this, new SwitchCameraStateOnMainMenuPlatform());
        }

        private void OnSwitchToGameStateToPlay(object sender, SwitchGameStateToPlayGameEvent eventData) 
        {
            HideAllViewsInList();
            EventAggregator.Post(this, new SwitchCameraStateOnMainMenuPlatform());
        }

        private void OnClickedShopSkinsButton()
        {
            HideOtherViewsAndShow(_shopSkinsPresenter);
            EventAggregator.Post(this, new SwitchCameraStateOnMainMenuShopSkins());
        }
        
        private void OnClickedPlatButton() => EventAggregator.Post(_mainMenuPresenter, new SwitchGameStateToPlayGameEvent());
        
    }
}