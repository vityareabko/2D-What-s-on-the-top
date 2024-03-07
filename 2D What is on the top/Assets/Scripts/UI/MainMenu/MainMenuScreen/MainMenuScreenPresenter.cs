using System;
using GameSM;
using UI.MVP;

namespace UI.MainMenu
{
    public interface IMainMenuPresenter : IPresenter < IMainMenuModel, MainMenuScreenView>
    {
        public event System.Action ClickedPlayButton;
        public event System.Action ClickedShopSkinsButton;
        public void OnClickedPlayButton();
        public void OnClickedShopSkinsButton();
    }

    public class MainMenuScreenPresenter : IMainMenuPresenter
    {
        public event Action ClickedPlayButton;
        public event Action ClickedShopSkinsButton;
        
        public IMainMenuModel Model { get; }
        public MainMenuScreenView View { get; }

        private bool _isInit = false; 
        
        public MainMenuScreenPresenter(IMainMenuModel model, MainMenuScreenView view)
        {
            Model = model;
            View = view;
            Init();
        }
        
        public void Show() => View.Show();
        
        public void Hide() => View.Hide();

        public void Init()
        {
            if (_isInit)
                return;

            _isInit = true;
            View.InitPresentor(this);
        }
        
        public void OnClickedPlayButton() => ClickedPlayButton?.Invoke();

        public void OnClickedShopSkinsButton() => ClickedShopSkinsButton?.Invoke();
    }
}

