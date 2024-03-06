using System;
using GameSM;
using UI.MVP;

namespace UI.MainMenu
{
    public interface IMainMenuPresenter : IPresenter < IMainMenuModel, MainMenuVieww>
    {
        public event System.Action ClickedPlayButton;
        public event System.Action ClickedShopSkinsButton;
        public void OnClickedPlayButton();
        public void OnClickedShopSkinsButton();
    }

    public class MainMenuPresenter : IMainMenuPresenter
    {
        public event Action ClickedPlayButton;
        public event Action ClickedShopSkinsButton;
        
        public IMainMenuModel Model { get; }
        public MainMenuVieww View { get; }

        private bool _isInit = false; 
        
        public MainMenuPresenter(IMainMenuModel model, MainMenuVieww view)
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

