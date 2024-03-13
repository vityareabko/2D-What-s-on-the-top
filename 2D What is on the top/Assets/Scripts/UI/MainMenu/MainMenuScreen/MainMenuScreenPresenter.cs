using System;
using PersistentData;
using UI.MVP;
using UnityEngine;

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

        private IPersistentResourceData _persistentResource;  
        
        private bool _isInit = false; 
        
        public MainMenuScreenPresenter(IMainMenuModel model, MainMenuScreenView view, IPersistentResourceData persistentResourceData)
        {
            Model = model;
            View = view;
            
            _persistentResource = persistentResourceData;
            OnResourceChanges(ResourceTypes.Coin);
            OnResourceChanges(ResourceTypes.Gem);
            
            _persistentResource.ResourcesJsonData.ResourceChange += OnResourceChanges; 
            Init();
        }
        
        public void Show() => View.Show();

        public void Hide(Action callBack = null) => View.Hide(callBack);
        
        public void Init()
        {
            if (_isInit)
                return;

            _isInit = true;
            View.InitPresentor(this);
        }
        
        public void OnClickedPlayButton() => ClickedPlayButton?.Invoke();

        public void OnClickedShopSkinsButton() => ClickedShopSkinsButton?.Invoke();

        private void OnResourceChanges(ResourceTypes type)
        {
            var resourcesChangesAmount = _persistentResource.ResourcesJsonData.GetResourcesAmountByType(type);

            switch (type)
            {
                case ResourceTypes.Coin:
                    View.SetCoinsAmount(resourcesChangesAmount);
                    break;
                case ResourceTypes.Gem:
                    View.SetCristtalAmount(resourcesChangesAmount);
                    break;
                default:
                    Debug.Log("Don't need To Change View, couse the type resource not on the View");
                    break;
            }
            
        }
    }
}

