using UI.MVP;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public interface IIMainMenuView : IView <IMainMenuPresenter>
    {
    }

    public class MainMenuScreenView : BaseScreenView, IIMainMenuView
    {

        public override ScreenType ScreenType { get; } = ScreenType.MainMenu;
        
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _shopSkinButton;
        
        public IMainMenuPresenter Presentor { get; private set; }
        
        public void InitPresentor(IMainMenuPresenter presentor) => Presentor = presentor;
        
        private void OnEnable()
        {
            _playButton.onClick.AddListener(OnPlayButtonClicked);
            _shopSkinButton.onClick.AddListener(OnShopSkinButtonClicked);
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveListener(OnPlayButtonClicked);
            _shopSkinButton.onClick.RemoveListener(OnShopSkinButtonClicked);
        }

        private void OnPlayButtonClicked() => Presentor.OnClickedPlayButton();

        private void OnShopSkinButtonClicked() => Presentor.OnClickedShopSkinsButton();
    }
}