using System;
using System.Linq;
using Game.Gameplay;
using Obstacles;
using PersistentData;
using UI.MVP;
using UnityEngine;

namespace UI.MainMenu
{
    public interface IMainMenuPresenter : IPresenter < IMainMenuModel, MainMenuScreenView>
    {
        public event Action ClickedPlayButton;
        public event Action ClickedShopSkinsButton;
        
        public void OnClickedPlayButton();
        public void OnClickedShopSkinsButton();
        public void OnLevelSelectedType(LevelType type);

        public void UpdateLevelsItems();
    }

    public class MainMenuScreenPresenter : IMainMenuPresenter
    {
        public event Action ClickedPlayButton;
        public event Action ClickedShopSkinsButton;
        
        public IMainMenuModel Model { get; }
        public MainMenuScreenView View { get; }

        private IPersistentResourceData _persistentResource;
        private IPersistentPlayerData _persistentPlayerData;
        private GameplayController _gameplayController;
        private LevelsDB _levelsDB;
        
        private bool _isInit = false; 
        
        public MainMenuScreenPresenter(
            IMainMenuModel model, 
            MainMenuScreenView view, 
            IPersistentResourceData persistentResourceData, 
            IPersistentPlayerData persistentPlayerData,
            GameplayController gameplayController,
            LevelsDB levelsDB
            )
        {
            Model = model;
            View = view;

            _levelsDB = levelsDB;
            _persistentPlayerData = persistentPlayerData;
            _persistentResource = persistentResourceData;
            _gameplayController = gameplayController;
            
            OnResourceChanges(ResourceTypes.Coin);
            OnResourceChanges(ResourceTypes.Gem);
            
            _persistentResource.ResourcesJsonData.ResourceChange += OnResourceChanges; 
            Init();
        }
        
        public void Show()
        {
            View.Show();
        }

        public void Hide(Action callBack = null) => View.Hide(callBack);
        
        public void Init()
        {
            if (_isInit)
                return;

            _isInit = true;
            View.InitPresentor(this);
        }
        
        public void UpdateLevelsItems()
        {
            foreach (var viewLevelButton in View.LevelItems)
            {
                viewLevelButton.Lock();
                viewLevelButton.Unselect();
                
                if (_persistentPlayerData.PlayerData.AvailableLevels.Contains(viewLevelButton.Type))
                    viewLevelButton.Unlock();
                
                if (_levelsDB.CurrentLevel == viewLevelButton.Type)
                    viewLevelButton.Select();
            }
        }
        
        public void OnClickedPlayButton() => ClickedPlayButton?.Invoke();

        public void OnClickedShopSkinsButton() => ClickedShopSkinsButton?.Invoke();
        
        public void OnLevelSelectedType(LevelType type)
        {
            if (_persistentPlayerData.PlayerData.AvailableLevels.Contains(type) == false ||
                _levelsDB.CurrentLevel == type) 
                return;
            
            _levelsDB.SetCurrentLevel(type);
            _persistentPlayerData.PlayerData.SetCurrentLevel(type);
            _gameplayController.MovePlayerToLevelSpawnPoint(type);
        }


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

