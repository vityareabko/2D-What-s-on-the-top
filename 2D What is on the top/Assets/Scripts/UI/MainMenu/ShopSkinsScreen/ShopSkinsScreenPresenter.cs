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
        // public event Action ClickBuyButton; 
        // public event Action ClickSelectButton; 
        public event Action ClickBackButton; 
        public void OnClickBuyButton();
        public void OnClickSelectButton();
        public void OnClickBackButton();
    }

    public class ShopSkinsScreenPresenter : IShopSkinsScreenPresenter, IDisposable
    {
        // public event Action ClickBuyButton;
        // public event Action ClickSelectButton;
        public event Action ClickBackButton;

        public IShopSkinsScreenModel Model { get; private set; }
        public IShopSkinsScreenView View { get; private set; }

        private bool _isInit;
        
        private List<ShopSkinItemView> _shopItems = new();
        private ShopSkinFactory _skinFactory;
        private ShopSkinDB _shopSkinDB;

        private ISelectSkin _selectSkin;
        private IUnlockerSkin _unlockerSkins;
        private IWalletResource _walletResource;

        private SkinItemConfig _previewSkin;
        
        private ShopSkinsScreenPresenter(IShopSkinsScreenModel model, 
            IShopSkinsScreenView view, 
            ShopSkinDB shopSkinDB, 
            ShopSkinFactory shopSkinFactory, 
            ISelectSkin selectSkin, 
            IUnlockerSkin unlockerSkins,
            IWalletResource walletResource)
        {
            Model = model;
            View = view;

            _shopSkinDB = shopSkinDB;
            _skinFactory = shopSkinFactory;
            _selectSkin = selectSkin;
            _unlockerSkins = unlockerSkins;
            _walletResource = walletResource;
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

        private void GenerateShopContent()
        {
            Clear();
            foreach (var skinItemConfig in _shopSkinDB.Skins)
            {
                var instance = _skinFactory.Get(skinItemConfig, View.ContentTransform);
                instance.ClickedOnView += OnClickeSkinItemView;
                _shopItems.Add(instance);
                
                if (_selectSkin.CurrentSkin == instance.Item.Type)
                {
                    instance.Select();
                    OnClickeSkinItemView(instance.Item);
                }
                
                if (_unlockerSkins.UnlockSkins.Contains(instance.Item.Type))
                    instance.Unlock();
                
                if (_unlockerSkins.UnlockSkins.Contains(instance.Item.Type) == false)
                    instance.Lock();
            }

            CheckOnEnoughMoneyForItems();
        }

        private void Clear()
        {
            foreach (var item in _shopItems)
                item.gameObject.Destroy();
            
            _shopItems.Clear();
        }

        private bool BuySkin() => _walletResource.Spend(_previewSkin.ResourceTypeByBuySkin, _previewSkin.PriceCoin);

        private void CheckOnEnoughMoneyForItems()
        {
            foreach (var item in _shopItems)
            {
                if(_walletResource.HasEnoughResourceAmount(item.Item.ResourceTypeByBuySkin, item.Item.PriceCoin))
                    item.DefaultPriceTextColor();
                else
                    item.RedPriceTextColor();
            }
        }

        private void OnClickeSkinItemView(SkinItemConfig itemView)
        {
            EventAggregator.Post(this, new TryOnSkinEvent() {TypeSkin = itemView.Type});

            _previewSkin = itemView;
            
            if (_walletResource.HasEnoughResourceAmount(itemView.ResourceTypeByBuySkin, itemView.PriceCoin))
                View.DefaultPriceColor();
            else
                View.RedPriceTextColor();
            
            if (_selectSkin.CurrentSkin == itemView.Type)
                View.ShowSelectedText();
            
            if (_unlockerSkins.UnlockSkins.Contains(itemView.Type) && (_selectSkin.CurrentSkin != itemView.Type))
                View.ShowSelectButton();
            
            if (_unlockerSkins.UnlockSkins.Contains(itemView.Type) == false)
                View.ShowBuyButton(itemView.PriceCoin);
            
        }

        public void OnClickBuyButton()
        {
            if (BuySkin() == false) 
            {
                // оповистить игрока
                // View.ShakeBuyButton 
                
                Debug.Log("Don't have enough money");
                
                return;
            }

            _unlockerSkins.Unlock(_previewSkin.Type);
            _selectSkin.Select(_previewSkin.Type);
            
            GenerateShopContent();
        }
        

        public void OnClickSelectButton()
        {
            // ClickSelectButton?.Invoke();
            _selectSkin.Select(_previewSkin.Type);
            GenerateShopContent();
            
        }

        public void OnClickBackButton()
        {
            EventAggregator.Post(this, new ApplySelectedSkinEvent() {CurrentSkin = _selectSkin.CurrentSkin} );
            ClickBackButton?.Invoke();
        }

        public void Dispose()
        {
            foreach (var item in _shopItems)
                item.ClickedOnView -= OnClickeSkinItemView;
        }
    }
}