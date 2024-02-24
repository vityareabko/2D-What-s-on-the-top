using System;
using System.Collections.Generic;
using UI.MVP;
using UnityEngine;
using Zenject;

namespace UI.MainMenu.MainMenuScreen
{
    public class MainMenuScreenUIController : MonoBehaviour
    {
        private IMainMenuScreenPresenter _mainMenuScreen;

        private List<IPresenter> _presenters = new ();

        private SceneLoadMediator _sceneLoader;
        
        [Inject] private void Construct(IMainMenuScreenPresenter mainMenuScreen, SceneLoadMediator sceneLoader)
        {
            _mainMenuScreen = mainMenuScreen;
            _sceneLoader = sceneLoader;
        }

        private void Awake()
        {
            _presenters.Add(_mainMenuScreen);
        }

        private void OnEnable()
        {
            _mainMenuScreen.ClickedModifyCharacterButton += OnClickedModifyCharacterButton;
            _mainMenuScreen.ClickedSelectLevelButton += OnClickedSelectLevelButton;
        }

        private void OnDisable()
        {
            _mainMenuScreen.ClickedModifyCharacterButton -= OnClickedModifyCharacterButton;
            _mainMenuScreen.ClickedSelectLevelButton -= OnClickedSelectLevelButton;
        }
        
        private void OnClickedSelectLevelButton()
        {
            // переход сцены
            _sceneLoader.GoToLevelSelection();
        }

        private void OnClickedModifyCharacterButton()
        {
            Debug.Log("Clicked Modify Character Button");
        }

    }
}