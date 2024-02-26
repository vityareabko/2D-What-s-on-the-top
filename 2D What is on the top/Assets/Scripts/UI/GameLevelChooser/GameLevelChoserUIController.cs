using System;
using UI.GameLevelChooser.ChooserLevel;
using UnityEngine;
using Zenject;

namespace UI.GameLevelChooser
{
    public class GameLevelChoserUIController : MonoBehaviour // # todo - можно его делать и не монобех а обычным и просто забиндит на сцене 
    {
        private LevelDatabases _levelDatabases;
        private SceneLoadMediator _sceneLoader;
        
        private IGameLevelChooserPresenter _chooserLevelPresenter;
        
        
        [Inject] private void Construct(
            LevelDatabases levelDatabases,
            SceneLoadMediator sceneLoadMediator,
            IGameLevelChooserPresenter chooseLevelPresenter)
        {
            _levelDatabases = levelDatabases;
            _sceneLoader = sceneLoadMediator;
            _chooserLevelPresenter = chooseLevelPresenter;
        }

        private void OnEnable() => _chooserLevelPresenter.OnPlayButtonClicked += OnLevelIsSelectedNeedToGo;
        private void OnDisable() => _chooserLevelPresenter.OnPlayButtonClicked -= OnLevelIsSelectedNeedToGo;

        private void OnLevelIsSelectedNeedToGo(LevelType type)
        {
            if (_levelDatabases.LevelConfigs.ContainsKey(type) == false) 
                throw new ArgumentException($"level {type} - couldn't founded");
            
            var levelConfig = _levelDatabases.LevelConfigs[type];
            _sceneLoader.GoToGameplay(levelConfig);
        }
    }
}