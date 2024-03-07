using UI.MainMenu.ShopSkinsScreen;
using UnityEngine;
using Zenject;

namespace UI.MainMenu
{
    public class MainMenuUIController : MonoBehaviour
    {
        private IMainMenuPresenter _mainMenuPresenter;
        private IShopSkinsScreenPresenter _shopSkinsPresenter;
        
        [Inject] private void Construct(IMainMenuPresenter mainMenuPresenter, IShopSkinsScreenPresenter shopSkinsPresenter)
        {
            _mainMenuPresenter = mainMenuPresenter;
            _shopSkinsPresenter = shopSkinsPresenter;
        }

        private void OnEnable()
        {
            _mainMenuPresenter.ClickedPlayButton += OnClickedPlatButton;
            _mainMenuPresenter.ClickedShopSkinsButton += OnClickedShopSkinsButton;
            EventAggregator.Subscribe<SwitchGameStateToPlayGameEvent>(OnSwitchToGameStateToPlay);
            EventAggregator.Subscribe<SwitchGameStateToMainMenuGameEvent>(OnSwitchToGameStateMainMenu);
        }


        private void OnDisable()
        {
            _mainMenuPresenter.ClickedPlayButton -= OnClickedPlatButton;
            _mainMenuPresenter.ClickedShopSkinsButton -= OnClickedShopSkinsButton;
            EventAggregator.Unsubscribe<SwitchGameStateToPlayGameEvent>(OnSwitchToGameStateToPlay);
            EventAggregator.Unsubscribe<SwitchGameStateToMainMenuGameEvent>(OnSwitchToGameStateMainMenu);
        }

        private void OnSwitchToGameStateMainMenu(object sender, SwitchGameStateToMainMenuGameEvent eventData)
        {
            EventAggregator.Post(this, new SwitchCameraStateOnMainMenuPlatform());
            _mainMenuPresenter.Show();
            _shopSkinsPresenter.Hide();
        }

        private void OnSwitchToGameStateToPlay(object sender, SwitchGameStateToPlayGameEvent eventData) 
        {
            EventAggregator.Post(this, new SwitchCameraStateOnMainMenuPlatform());
            _mainMenuPresenter.Hide();
        }

        private void OnClickedShopSkinsButton()
        {
            EventAggregator.Post(this, new SwitchCameraStateOnMainMenuShopSkins());
            _mainMenuPresenter.Hide();
            _shopSkinsPresenter.Show();
            // сдесь нужно открыть магазаин или Event на который магазин будет реагировать
        }
        
        private void OnClickedPlatButton() => EventAggregator.Post(_mainMenuPresenter, new SwitchGameStateToPlayGameEvent());
        
    }
}