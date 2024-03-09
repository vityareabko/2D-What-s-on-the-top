using System;
using System.Collections.Generic;
using MyNamespace.Services.StorageService.SelectorSkin;
using UI.MainMenu.ShopSkinItemPanel;
using UI.MVP;
using UnityEngine;
using UnlockerSkins;
using VHierarchy.Libs;
using WalletResources;

namespace UI.MainMenu.ShopSkinsScreen
{
    public interface IShopSkinsScreenPresenter : IPresenter<IShopSkinsScreenModel, IShopSkinsScreenView>
    {
        public event Action ClickBackButton; 
        public void OnClickBuyButton();
        public void OnClickSelectButton();
        public void OnClickBackButton();

        public void GenerateShopContent();
    }

    public class ShopSkinsScreenPresenter : IShopSkinsScreenPresenter, IDisposable
    {
        public event Action ClickBackButton;

        public IShopSkinsScreenModel Model { get; private set; }
        public IShopSkinsScreenView View { get; private set; }

        private bool _isInit;
        
        private List<ShopSkinItemView> _shopSkinsItems = new();
        // private List<ShopSkinItemView> _shopShieldSkinsItems = new(); // возможно это лишний так как можно все делать в _shopSkinsItems
        
        private ShopSkinFactory _skinFactory;
        private ShopSkinDB _shopSkinDB;

        private ISelectSkin _selectSkin;
        private IUnlockerSkin _unlockerSkins;
        private IWalletResource _walletResource;

        private SkinItemConfig _previewSkin;
        
        private ShopSkinsScreenPresenter(
            IShopSkinsScreenModel model, 
            IShopSkinsScreenView view, 
            ISelectSkin selectSkin, 
            IUnlockerSkin unlockerSkins,
            IWalletResource walletResource,
            ShopSkinDB shopSkinDB,
            ShopSkinFactory shopSkinFactory) 
        {
            Model = model;
            View = view;
            
            _skinFactory = shopSkinFactory;
            _selectSkin = selectSkin;
            _unlockerSkins = unlockerSkins;
            _walletResource = walletResource;
            _shopSkinDB = shopSkinDB;
            Init();
        }

        public void Init()
        {
            if (_isInit)
                return;

            _isInit = true;
            View.InitPresentor(this);
        }

        public void Show()
        {
            View.Show();
            GenerateShopContent();
        }

        public void Hide() => View.Hide();

        public void GenerateShopContent()
        {
            Clear();
         
            switch (View.ActiveSkinTab)
            {
                case ShopSkinTabType.HeroTab:
                    // генерируем контент скинов
                    GenerateHeroSkinContent(_shopSkinDB.HeroSkins, View.HeroSkinsContent);
                    break;
                case ShopSkinTabType.ShieldTab:
                    GenerateHeroSkinContent(_shopSkinDB.ShieldSkins, View.ShieldSkinsContent);
                    // генерируем контет щитов
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            CheckOnEnoughMoneyForItems();
        }

        private void GenerateHeroSkinContent(List<SkinItemConfig> skins, Transform contentTransform)
        {
            foreach (var skinItemConfig in skins)
            {
                var instance = _skinFactory.Get(skinItemConfig, contentTransform);
                instance.ClickedOnView += OnClickeSkinItemView;
                _shopSkinsItems.Add(instance);
                
                if (_unlockerSkins.UnlockSkins.Contains(instance.Item.Type))
                    instance.Unlock();
                
                if (_unlockerSkins.UnlockSkins.Contains(instance.Item.Type) == false)
                    instance.Lock();
                
                switch (View.ActiveSkinTab)
                {
                    case ShopSkinTabType.HeroTab:
                        if (_selectSkin.CurrentHeroSkin == instance.Item.Type)
                        {
                            instance.Select(); // item 
                            OnClickeSkinItemView(instance.Item); // change skin
                            Model.SelectSkin(instance.Item.Type, View.ActiveSkinTab); // screen buttons update
                        }
                        break;
                    
                    case ShopSkinTabType.ShieldTab:
                        if (_selectSkin.CurrentShieldSkin == instance.Item.Type)
                        {
                            instance.Select();
                            OnClickeSkinItemView(instance.Item);
                            Model.SelectSkin(instance.Item.Type, View.ActiveSkinTab);
                            
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void Clear()
        {
            foreach (var item in _shopSkinsItems)
                item.gameObject.Destroy();
            
            _shopSkinsItems.Clear();
        }

        private void CheckOnEnoughMoneyForItems()
        {
            foreach (var item in _shopSkinsItems)
            {
                if(_walletResource.HasEnoughResourceAmount(item.Item.ResourceTypeByBuySkin, item.Item.PriceCoin))
                    item.DefaultPriceTextColor();
                else
                    item.RedPriceTextColor();
            }
        }

        private void OnClickeSkinItemView(SkinItemConfig itemView)
        {
            switch (View.ActiveSkinTab)
            {
                case ShopSkinTabType.HeroTab:
                    EventAggregator.Post(this, new TryOnSkinEvent() {Skin = itemView.Type});
                    break;
                case ShopSkinTabType.ShieldTab:
                    EventAggregator.Post(this, new TryOnShieldSkinEvent() {Skin = itemView.Type});
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _previewSkin = itemView;
            
            if (_walletResource.HasEnoughResourceAmount(itemView.ResourceTypeByBuySkin, itemView.PriceCoin))
                View.DefaultPriceColor();
            else
                View.RedPriceTextColor();
            
            switch (View.ActiveSkinTab)
            {
                case ShopSkinTabType.HeroTab:
                    
                    if (_selectSkin.CurrentHeroSkin == itemView.Type)
                        View.ShowSelectedText();
                    
                    if (_unlockerSkins.UnlockSkins.Contains(itemView.Type) && (_selectSkin.CurrentHeroSkin != itemView.Type))
                        View.ShowSelectButton();
                    
                    break;
                case ShopSkinTabType.ShieldTab:
                    
                    if (_selectSkin.CurrentShieldSkin == itemView.Type)
                        View.ShowSelectedText();
                    
                    if (_unlockerSkins.UnlockSkins.Contains(itemView.Type) && (_selectSkin.CurrentShieldSkin != itemView.Type))
                        View.ShowSelectButton();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            
            
            if (_unlockerSkins.UnlockSkins.Contains(itemView.Type) == false)
                View.ShowBuyButton(itemView.PriceCoin);
            
        }

        public void OnClickBuyButton()
        {
            if (Model.TryBuySkin(_previewSkin, View.ActiveSkinTab))
                GenerateShopContent();
        }

        public void OnClickSelectButton()
        {
            Model.SelectSkin(_previewSkin.Type, View.ActiveSkinTab);
            GenerateShopContent();
        }

        public void OnClickBackButton()
        {
            EventAggregator.Post(this, new ApplySelectedHeroSkinEvent() {CurrentSkin = _selectSkin.CurrentHeroSkin} );
            ClickBackButton?.Invoke();
        }

        public void Dispose()
        {
            foreach (var item in _shopSkinsItems)
                item.ClickedOnView -= OnClickeSkinItemView;
        }
    }
}