using System;
using UI.MVP;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu.MainMenuScreen
{
    public interface IMainMenuScreenView : IView<IMainMenuScreenPresenter>
    {
    }

    public class MainMenuScreenView : BaseScreenView, IMainMenuScreenView
    {
        public override ScreenType ScreenType { get; } = ScreenType.MainMenu;
        
        [SerializeField] private Button _goToModifyCharacterButton;
        [SerializeField] private Button _goToSelectLevelsButton;

        public IMainMenuScreenPresenter Presentor { get; private set; }
        
        public void InitPresentor(IMainMenuScreenPresenter presentor)
        {
            Presentor = presentor;
        }
        
        private void OnEnable()
        {
            _goToModifyCharacterButton.onClick.AddListener(OnClickedModifyCharcterButton);
            _goToSelectLevelsButton.onClick.AddListener(OnClickedSelectLevelsButton);
        }

        private void OnDisable()
        {
            _goToModifyCharacterButton.onClick.RemoveListener(OnClickedModifyCharcterButton);
            _goToSelectLevelsButton.onClick.RemoveListener(OnClickedSelectLevelsButton);
        }

        private void OnClickedSelectLevelsButton() => Presentor.OnClickSelectLevelButton();
        
        private void OnClickedModifyCharcterButton() => Presentor.OnClikedModifyCharacterButton();
        

        
        
    }
}