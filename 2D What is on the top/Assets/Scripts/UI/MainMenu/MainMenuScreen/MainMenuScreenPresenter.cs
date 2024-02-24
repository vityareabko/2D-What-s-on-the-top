using System;
using UI.MVP;
using UnityEngine;

namespace UI.MainMenu.MainMenuScreen
{
    public interface IMainMenuScreenPresenter : IPresenter<IMainMenuScreenModel, IMainMenuScreenView>
    {
        public event Action ClickedModifyCharacterButton; 
        public event Action ClickedSelectLevelButton; 
        public void OnClikedModifyCharacterButton();
        public void OnClickSelectLevelButton();
    }

    public class MainMenuScreenPresenter : IMainMenuScreenPresenter
    {
        public event Action ClickedModifyCharacterButton;
        public event Action ClickedSelectLevelButton;
        
        public IMainMenuScreenModel Model { get; }
        public IMainMenuScreenView View { get;}
        
        private bool _isInit = false;
        
        public MainMenuScreenPresenter(IMainMenuScreenModel model, IMainMenuScreenView view)
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

        public void OnClickSelectLevelButton()
        {
            // оповистить что нужно сделать переход на сцену SelectLevels порробовать через EvenAggregator 
            // а также черех обычный Event
            // и сравнить 
            
            ClickedSelectLevelButton?.Invoke();
            
        }

        

        public void OnClikedModifyCharacterButton() => ClickedModifyCharacterButton?.Invoke();
        
    }
}