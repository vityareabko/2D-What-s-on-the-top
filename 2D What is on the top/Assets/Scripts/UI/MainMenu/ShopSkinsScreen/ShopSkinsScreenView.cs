using System;
using Extensions;
using TMPro;
using UI.MVP;
using UnityEngine;
using DG.Tweening;

using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace UI.MainMenu.ShopSkinsScreen
{
    public interface IShopSkinsScreenView : IView<IShopSkinsScreenPresenter>
    {
        public Transform HeroSkinsContent { get; }
        public Transform ShieldSkinsContent { get; }

        public ShopSkinTabType ActiveSkinTab { get; }

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
        
        [SerializeField] private RectTransform _topPanelRectTransform;
        [SerializeField] private RectTransform _buttonsRectTransform;
        [SerializeField] private TabsUIController _tabsUIController;
        
        [SerializeField] private Transform _heroSkinsContent;
        [SerializeField] private Transform _shieldSkinsContetn;

        [SerializeField] private Button _heroSkinsTabButton;
        [SerializeField] private Button _shieldSkinsTabButton;

        [SerializeField] private Button _selectSkinButton;
        [SerializeField] private Button _buySkinButton;
        [SerializeField] private Image _selectedText;

        [SerializeField] private TMP_Text _priceBuyButtonText;
        [SerializeField] private Color _colorDefault;
        [SerializeField] private Color _colorDosentEnoughMoney;

        [SerializeField] private Button _backButton;

        private ShopSkinTabType _currentActiveSkinTab = ShopSkinTabType.HeroTab;

        public ShopSkinTabType ActiveSkinTab => _currentActiveSkinTab;

        public IShopSkinsScreenPresenter Presentor { get; set; }

        public void InitPresentor(IShopSkinsScreenPresenter presentor) => Presentor = presentor;

        public Transform HeroSkinsContent => _heroSkinsContent;
        public Transform ShieldSkinsContent => _shieldSkinsContetn;

        protected override void OnShow()
        {
            base.OnShow();

            Vector2 showPositionButtons = new Vector2(0, _buttonsRectTransform.anchoredPosition.y * -1f); 
            Vector2 showPositionTabPanel = new Vector2(_tabsUIController.GetComponent<RectTransform>().anchoredPosition.x * -1f, 0);
            Vector2 showPostionTopPanel = new Vector2(0, _topPanelRectTransform.anchoredPosition.y * -1f);
            
            AnimateElement(_buttonsRectTransform, showPositionButtons);
            AnimateElement(_tabsUIController.GetComponent<RectTransform>(), showPositionTabPanel);
            AnimateElement(_topPanelRectTransform, showPostionTopPanel);
        }
        
        public override void Hide(Action callBack)
        {
            int completedAnimations = 0;
            int totalAnimations = 3; 
            
            Action onAllAnimationsComplete = () =>
            {
                completedAnimations++;
                if (completedAnimations == totalAnimations)
                {
                    callBack?.Invoke();
                    base.Hide(); 
                }
            };
            
            Vector2 hidePositionButtons = new Vector2(0, _buttonsRectTransform.anchoredPosition.y * -1f);
            Vector2 hidePositionTabPanel = new Vector2(_tabsUIController.GetComponent<RectTransform>().anchoredPosition.x * -1f, 0);
            Vector2 hidePositionTopPanel = new Vector2(0, _topPanelRectTransform.anchoredPosition.y * -1f);
    
            AnimateElementHide(_buttonsRectTransform, hidePositionButtons, onAllAnimationsComplete);
            AnimateElementHide(_tabsUIController.GetComponent<RectTransform>(), hidePositionTabPanel, onAllAnimationsComplete);
            AnimateElementHide(_topPanelRectTransform, hidePositionTopPanel, onAllAnimationsComplete);
        }
        
        private void AnimateElement(RectTransform rectTransform, Vector2 targetPosition, Action callback = null) =>
            rectTransform.DOAnchorPos(targetPosition, 0.6f).SetEase(Ease.OutQuad).OnComplete(() => callback?.Invoke());
        
        private void AnimateElementHide(RectTransform rectTransform, Vector2 originalPosition, Action callback = null) =>
            rectTransform.DOAnchorPos(originalPosition, 0.6f).SetEase(Ease.InQuad).OnComplete(() => callback?.Invoke());

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

        public void DefaultPriceColor() => _priceBuyButtonText.color = _colorDefault;

        public void RedPriceTextColor() => _priceBuyButtonText.color = _colorDosentEnoughMoney;

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