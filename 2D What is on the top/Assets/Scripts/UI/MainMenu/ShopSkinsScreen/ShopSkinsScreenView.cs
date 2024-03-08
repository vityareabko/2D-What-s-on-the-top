using Assets.HeroEditor.Common.Scripts.Common;
using Extensions;
using TMPro;
using UI.MVP;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu.ShopSkinsScreen
{
    public interface IShopSkinsScreenView : IView<IShopSkinsScreenPresenter>
    {
        public Transform ContentTransform { get; }

        public void ShowSelectedText();
        public void ShowSelectButton();
        public void ShowBuyButton(int amount);

        public void DefaultPriceColor();
        public void RedPriceTextColor();
    }

    public class ShopSkinsScreenView : BaseScreenView, IShopSkinsScreenView
    {
        public override ScreenType ScreenType => ScreenType.ShopSkins;

        [SerializeField] private Transform _panelContent;
        
        [SerializeField] private Button _selectSkinButton;
        [SerializeField] private Button _buySkinButton;
        [SerializeField] private Image _selectedText;

        [SerializeField] private TMP_Text _priceBuyButtonText;
        [SerializeField] private Color _colorDefault;
        [SerializeField] private Color _colorDosentEnoughMoney;
        
        [SerializeField] private Button _backButton;

        public IShopSkinsScreenPresenter Presentor { get; set; }
        
        public void InitPresentor(IShopSkinsScreenPresenter presentor) => Presentor = presentor;

        public Transform ContentTransform => _panelContent;

        private void OnEnable()
        {
            _selectSkinButton.onClick.AddListener(OnClickedSelectSkinButton);
            _buySkinButton.onClick.AddListener(OnClickedBuySkinButton);
            _backButton.onClick.AddListener(OnClickedBackButton);
        }

        private void OnDisable()
        {
            _selectSkinButton.onClick.RemoveListener(OnClickedSelectSkinButton);
            _buySkinButton.onClick.RemoveListener(OnClickedBuySkinButton);
            _backButton.onClick.RemoveListener(OnClickedBackButton);
        }
        
        public void ShowSelectedText()
        {
            _selectedText.SetActive(true);
            _selectSkinButton.SetActive(false);
            _buySkinButton.SetActive(false);
        }

        public void ShowBuyButton(int amount)
        {
            _priceBuyButtonText.Show(amount);
            _selectedText.SetActive(false);
            _selectSkinButton.SetActive(false);
            _buySkinButton.SetActive(true);
        }

        public void ShowSelectButton()
        {
            _selectedText.SetActive(false);
            _selectSkinButton.SetActive(true);
            _buySkinButton.SetActive(false);
        }

        public void DefaultPriceColor() => _priceBuyButtonText.color = _colorDefault;

        public void RedPriceTextColor() => _priceBuyButtonText.color = _colorDosentEnoughMoney;

        private void OnClickedBackButton() => Presentor.OnClickBackButton();

        private void OnClickedSelectSkinButton() => Presentor.OnClickSelectButton();

        private void OnClickedBuySkinButton() => Presentor.OnClickBuyButton();
        
    }
}