using UI.MVP;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public interface IIMainMenuView : IView <IMainMenuPresenter>
    {
    }

    public class MainMenuView : BaseScreenView, IIMainMenuView
    {

        public override ScreenType ScreenType { get; } = ScreenType.MainMenu;
        
        [SerializeField] private Button _playButton;
        
        public IMainMenuPresenter Presentor { get; private set; }
        
        public void InitPresentor(IMainMenuPresenter presentor) => Presentor = presentor;
        
        private void OnEnable() =>
            _playButton.onClick.AddListener(OnPlayButtonClicked);
        
        private void OnDisable() =>
            _playButton.onClick.AddListener(OnPlayButtonClicked);
        
        private void OnPlayButtonClicked() => Presentor.OnClickedPlayButton();

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}