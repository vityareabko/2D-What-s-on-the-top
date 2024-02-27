using UnityEngine;
using Zenject;

namespace UI.MainMenu
{
    public class MainMenuUIController : MonoBehaviour
    {
        private IMainMenuPresenter _mainMenuPresenter;
        
        [Inject] private void Construct(IMainMenuPresenter mainMenuPresenter)
        {
            _mainMenuPresenter = mainMenuPresenter;
        }

        private void OnEnable()
        {
            _mainMenuPresenter.ClickedPlayButton += OnClickedPlatButton;
            EventAggregator.Subscribe<SwitchGameStateToPlayGameEvent>(OnSwitchToGameStateToPlay);
            EventAggregator.Subscribe<SwitchGameStateToMainMenuGameEvent>(OnSwitchToGameStateMainMenu);
        }


        private void OnDisable()
        {
            _mainMenuPresenter.ClickedPlayButton -= OnClickedPlatButton;
            EventAggregator.Unsubscribe<SwitchGameStateToPlayGameEvent>(OnSwitchToGameStateToPlay);
            EventAggregator.Unsubscribe<SwitchGameStateToMainMenuGameEvent>(OnSwitchToGameStateMainMenu);
        }

        private void OnSwitchToGameStateMainMenu(object sender, SwitchGameStateToMainMenuGameEvent eventData) =>
            _mainMenuPresenter.Show();
        
        private void OnSwitchToGameStateToPlay(object sender, SwitchGameStateToPlayGameEvent eventData) 
        {
            _mainMenuPresenter.Hide();
        }
        
        private void OnClickedPlatButton() => EventAggregator.Post(_mainMenuPresenter, new SwitchGameStateToPlayGameEvent());
        
    }
}