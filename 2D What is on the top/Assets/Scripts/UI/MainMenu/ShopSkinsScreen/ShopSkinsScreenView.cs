using UI.MVP;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu.ShopSkinsScreen
{
    public interface IShopSkinsScreenView : IView<IShopSkinsScreenPresenter>
    {
        public Transform ContentTransform { get; }
    }

    public class ShopSkinsScreenView : BaseScreenView, IShopSkinsScreenView
    {
        public override ScreenType ScreenType => ScreenType.ShopSkins;

        [SerializeField] private Transform _panelContent;
        [SerializeField] private Button _selectSkinButton;
        [SerializeField] private Button _buySkinButton;

        public IShopSkinsScreenPresenter Presentor { get; set; }
        
        public void InitPresentor(IShopSkinsScreenPresenter presentor) => Presentor = presentor;

        public Transform ContentTransform => _panelContent;

        private void OnEnable()
        {
            _selectSkinButton.onClick.AddListener(OnClickedSelectSkinButton);
            _buySkinButton.onClick.AddListener(OnClickedBuySkinButton);
        }

        private void OnDisable()
        {
            _selectSkinButton.onClick.RemoveListener(OnClickedSelectSkinButton);
            _buySkinButton.onClick.RemoveListener(OnClickedBuySkinButton);
        }

        private void OnClickedSelectSkinButton() => Presentor.OnClickSelectButton();

        private void OnClickedBuySkinButton() => Presentor.OnClickBuyButton();
    }
}