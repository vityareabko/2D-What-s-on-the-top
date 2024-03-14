using System;
using Extensions;
using TMPro;
using UI.MVP;
using UnityEngine;

using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace UI.MainMenu.ShopSkinsScreen
{
    public interface IShopSkinsScreenView : IView<IShopSkinsScreenPresenter>
    {
        public Transform HeroSkinsContent { get; }
        public Transform ShieldSkinsContent { get; }

        public ShopSkinTabType ActiveSkinTab { get; }
        
        public void SetCristtalAmount(int amount);
        public void SetCoinsAmount(int amount);
        
        public void ShowSelectedText();
        public void ShowSelectButton();
        public void ShowBuyButton(int amount);

        public void DefaultPriceColor();
        public void RedPriceTextColor();
    }

    public enum ShopSkinTabType
    {
        HeroTab,
        ShieldTab
    }
    
    public class ShopSkinsScreenView : BaseScreenView, IShopSkinsScreenView
    {
        public override ScreenType ScreenType => ScreenType.ShopSkins;
        
        [Header("RectTransforms")]
        [SerializeField] private RectTransform _topPanelRectTransform;
        [SerializeField] private RectTransform _buttonsRectTransform;
        
        [Header("Skin Category Content")]
        [SerializeField] private Transform _heroSkinsContent;
        [SerializeField] private Transform _shieldSkinsContetn;

        [Header("Buttons")]
        [SerializeField] private Button _heroSkinsTabButton;
        [SerializeField] private Button _shieldSkinsTabButton;
        [SerializeField] private Button _selectSkinButton;
        [SerializeField] private Button _buySkinButton;
        [SerializeField] private Image _selectedText;
        [SerializeField] private Button _backButton;
        
        [Header("Text")]
        [SerializeField] private TMP_Text _priceBuyButtonText;
        [SerializeField] private TMP_Text _cristalAmount;
        [SerializeField] private TMP_Text _coinsAmount;
        
        [Header("Color")]
        [SerializeField] private Color _colorDefault;
        [SerializeField] private Color _colorDosentEnoughMoney;

        [Header("TabController")]
        [SerializeField] private TabsUIController _tabsUIController;
        

        private RectTransform _tabPanelRectTransform;
        private ShopSkinTabType _currentActiveSkinTab = ShopSkinTabType.HeroTab;
        
        public IShopSkinsScreenPresenter Presentor { get; set; }

        public void InitPresentor(IShopSkinsScreenPresenter presentor) => Presentor = presentor;

        public ShopSkinTabType ActiveSkinTab => _currentActiveSkinTab;
        public Transform HeroSkinsContent => _heroSkinsContent;
        public Transform ShieldSkinsContent => _shieldSkinsContetn;

        protected override void OnAwake()
        {
            base.OnAwake();
            _tabPanelRectTransform = _tabsUIController.GetComponent<RectTransform>();
        }

        protected override void OnShow()
        {
            base.OnShow();
            _tabPanelRectTransform.AnimateFromOutsideToPosition(_tabPanelRectTransform.anchoredPosition, RectTransformExtensions.Direction.Right);
            _buttonsRectTransform.AnimateFromOutsideToPosition(_buttonsRectTransform.anchoredPosition, RectTransformExtensions.Direction.Down);
            _topPanelRectTransform.AnimateFromOutsideToPosition(_topPanelRectTransform.anchoredPosition, RectTransformExtensions.Direction.Up);
        }
        
        public override void Hide(Action callBack = null)
        {
            if (callBack == null)
            {
                base.Hide();
                return;
            }
            
            int completedAnimations = 0;
            int totalAnimations = 3;

            Action OnCompleted = () =>
            {
                completedAnimations++;
                if (completedAnimations == totalAnimations)
                {
                    callBack?.Invoke();
                    base.Hide();
                }
            };
            
            _tabPanelRectTransform.AnimateBackOutsideScreen(RectTransformExtensions.Direction.Right, callback: OnCompleted);
            _buttonsRectTransform.AnimateBackOutsideScreen(RectTransformExtensions.Direction.Down, callback: OnCompleted);
            _topPanelRectTransform.AnimateBackOutsideScreen(RectTransformExtensions.Direction.Up, callback: OnCompleted);
        }

        private void OnEnable()
        {
            _selectSkinButton.onClick.AddListener(OnClickedSelectSkinButton);
            _buySkinButton.onClick.AddListener(OnClickedBuySkinButton);
            _backButton.onClick.AddListener(OnClickedBackButton);
            
            _heroSkinsTabButton.onClick.AddListener(OnActivateHeroTabButton);
            _shieldSkinsTabButton.onClick.AddListener(OnActivateShielTabButton);
        }

        private void OnDisable()
        {
            _selectSkinButton.onClick.RemoveListener(OnClickedSelectSkinButton);
            _buySkinButton.onClick.RemoveListener(OnClickedBuySkinButton);
            _backButton.onClick.RemoveListener(OnClickedBackButton);
            
            _heroSkinsTabButton.onClick.RemoveListener(OnActivateHeroTabButton);
            _shieldSkinsTabButton.onClick.RemoveListener(OnActivateShielTabButton);
        }
        
        public void SetCristtalAmount(int amount) => _cristalAmount.Show(amount);

        public void SetCoinsAmount(int amount) => _coinsAmount.Show(amount);

        public void DefaultPriceColor() => _priceBuyButtonText.color = _colorDefault;

        public void RedPriceTextColor() => _priceBuyButtonText.color = _colorDosentEnoughMoney;
        
        public void ShowSelectedText()
        {
            _selectedText.gameObject.SetActive(true);
            _selectSkinButton.gameObject.SetActive(false);
            _buySkinButton.gameObject.SetActive(false);
        }

        public void ShowBuyButton(int amount)
        {
            _priceBuyButtonText.Show(amount);
            _selectedText.gameObject.SetActive(false);
            _selectSkinButton.gameObject.SetActive(false);
            _buySkinButton.gameObject.SetActive(true);
        }

        public void ShowSelectButton()
        {
            _selectedText.gameObject.SetActive(false);
            _selectSkinButton.gameObject.SetActive(true);
            _buySkinButton.gameObject.SetActive(false);
        }
        

        private void ResetView()
        {
            _tabsUIController.OnTabButtonClicked((int)ShopSkinTabType.HeroTab);
            _currentActiveSkinTab = ShopSkinTabType.HeroTab;
            
            ResetScrollPosition(_heroSkinsContent);
            ResetScrollPosition(_shieldSkinsContetn);
            
            Presentor.GenerateShopContent();
        }
        
        private void ResetScrollPosition(Transform contentTransform)
        {
            if (contentTransform.GetComponent<RectTransform>() is RectTransform rectTransform)
                rectTransform.anchoredPosition = Vector2.zero;
            
        }

        private void OnClickedBackButton()
        {
            
            if (ActiveSkinTab == ShopSkinTabType.ShieldTab)
                EventAggregator.Post(this, new EndShowShieldSkinEvent());
            
            ResetView();
            
            Presentor.OnClickBackButton();
        }

        private void OnClickedSelectSkinButton() => Presentor.OnClickSelectButton();

        private void OnClickedBuySkinButton() => Presentor.OnClickBuyButton();

        private void OnActivateShielTabButton()
        {
            ResetScrollPosition(_shieldSkinsContetn);
            EventAggregator.Post(this, new StartShowShieldSkinEvent());
            _currentActiveSkinTab = ShopSkinTabType.ShieldTab;
            Presentor.GenerateShopContent();
        }

        private void OnActivateHeroTabButton()
        {
            ResetScrollPosition(_heroSkinsContent);
            EventAggregator.Post(this, new EndShowShieldSkinEvent());
            _currentActiveSkinTab = ShopSkinTabType.HeroTab;
            Presentor.GenerateShopContent();
        }
    }
}