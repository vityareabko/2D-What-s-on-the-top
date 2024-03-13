using System;
using System.Collections.Generic;
using System.Linq;
using PersistentData;
using Scriptable.Configs.ShopSkins.@base;
using ShopSkinVisitor.Visitable;
using UI.MainMenu.ShopSkinItemPanel;
using UI.MVP;
using UnityEngine;
using VHierarchy.Libs;


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
        
        private ShopSkinFactory _skinFactory;
        private ShopSkinDB _shopSkinDB;

        private IPersistentResourceData _walletResource;
        private IPersistentPlayerData _persistentData;

        private SkinItem _previewSkin;

        private OpenSkinChecher _openSkinChecker;
        private SelectSkinChecher _skinSeletChecker;
        private ClickSkinItemView _clickSkinItem;

        
        private ShopSkinsScreenPresenter(
            IShopSkinsScreenModel model, 
            IShopSkinsScreenView view, 

            IPersistentResourceData walletResource,
            IPersistentPlayerData persistentData,
            
            ShopSkinDB shopSkinDB,
            ShopSkinFactory shopSkinFactory, 
            
            OpenSkinChecher openSkinChecker,
            SelectSkinChecher skinSeletChecker,
            ClickSkinItemView clickSkinItem)
        {
            Model = model;
            View = view;
            
            _skinFactory = shopSkinFactory;

            _shopSkinDB = shopSkinDB;
            _walletResource = walletResource;
            _persistentData = persistentData;
            
            _openSkinChecker = openSkinChecker;
            _skinSeletChecker = skinSeletChecker;
            _clickSkinItem = clickSkinItem;
            
            OnResourceChange(ResourceTypes.Coin);
            OnResourceChange(ResourceTypes.Gem);

            _walletResource.ResourcesJsonData.ResourceChange += OnResourceChange;
            
            Init();
        }
        
        public void Dispose()
        {
            foreach (var item in _shopSkinsItems)
                item.ClickedOnView -= OnClickeSkinItemView;
        }

        private void OnResourceChange(ResourceTypes type)
        {
            var getAmountResource = _walletResource.ResourcesJsonData.GetResourcesAmountByType(type);
            
            switch (type)
            {
                case ResourceTypes.Coin:
                    View.SetCoinsAmount(getAmountResource);
                    break;
                case ResourceTypes.Gem:
                    View.SetCristtalAmount(getAmountResource);
                    break;
                default:
                    Debug.Log("Don't need To Change View, couse the type resource not on the View");
                    break;
            }
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
        
        public void Hide(Action callBack = null)
        {
            View.Hide(callBack);
        }

        public void GenerateShopContent()
        {
            Clear();
         
            switch (View.ActiveSkinTab)
            {
                case ShopSkinTabType.HeroTab:
                    GenerateHeroSkinContent(_shopSkinDB.HeroSkinItems, View.HeroSkinsContent);
                    break;
                case ShopSkinTabType.ShieldTab:
                    GenerateHeroSkinContent(_shopSkinDB.ShieldSkinItems, View.ShieldSkinsContent);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            CheckOnEnoughMoneyForItems();
        }
        
        private void GenerateHeroSkinContent(IEnumerable<SkinItem> items, Transform parent)
        {
            Clear();
            
            foreach (var item in items)
            {
                var spawnedItem = _skinFactory.Get(item, parent);
                spawnedItem.ClickedOnView += OnClickeSkinItemView;
                
                spawnedItem.Unselect();
                spawnedItem.Lock();

                ChekSkinOnOpenAndSelect(spawnedItem);
                
                _shopSkinsItems.Add(spawnedItem);
            }

            SortItems();
        }

        private void SortItems()
        {
            _shopSkinsItems = _shopSkinsItems.OrderBy(item => item.IsLock).ToList();
            for (int i = 0; i < _shopSkinsItems.Count; i++) 
                _shopSkinsItems[i].transform.SetSiblingIndex(i);
        }

        private void Clear()
        {
            Dispose();
            foreach (var item in _shopSkinsItems)
                item.gameObject.Destroy();

            _shopSkinsItems.Clear();
        }

        private void CheckOnEnoughMoneyForItems()
        {
            foreach (var item in _shopSkinsItems)
            {
                if(_walletResource.ResourcesJsonData.HasEnoughResourceAmount(item.Item.ResourceTypeByBuySkin, item.Item.PriceCoin))
                    item.DefaultPriceTextColor();
                else
                    item.RedPriceTextColor();
            }
        }

        private void ChekSkinOnOpenAndSelect(ShopSkinItemView item)
        {
            item.Item.Accept(_openSkinChecker);
            
            if (_openSkinChecker.IsOpen)
            {
                item.Item.Accept(_skinSeletChecker);
                if (_skinSeletChecker.IsSelect)
                {
                    item.Select();
                    OnClickeSkinItemView(item.Item);
                }
                
                item.Unlock();
            }
        }
        
        private void OnClickeSkinItemView(SkinItem item)
        {
            item.Accept(_clickSkinItem); 

            _previewSkin = item;

            item.Accept(_openSkinChecker);
            item.Accept(_skinSeletChecker);
            
            if (_walletResource.ResourcesJsonData.HasEnoughResourceAmount(item.ResourceTypeByBuySkin, item.PriceCoin))
                View.DefaultPriceColor();
            else
                View.RedPriceTextColor();
            
            if(_skinSeletChecker.IsSelect)
                View.ShowSelectedText();
            
            if (_openSkinChecker.IsOpen && (_skinSeletChecker.IsSelect == false))
                View.ShowSelectButton();
            
            if (_openSkinChecker.IsOpen == false)
                View.ShowBuyButton(item.PriceCoin);

        }
        
        public void OnClickBuyButton()
        {
            if (Model.TryBuySkin(_previewSkin))
            {
                _persistentData.SaveData();
                GenerateShopContent();
            }
        }

        public void OnClickSelectButton()
        {
            Model.SelectSkin(_previewSkin);
            _persistentData.SaveData();
            GenerateShopContent();
        }

        public void OnClickBackButton()
        {
            if (_previewSkin == null)
            {
                ClickBackButton?.Invoke();
                return;
            }

            _previewSkin.Accept(_skinSeletChecker);
            
            EventAggregator.Post(this, new ApplySelectedHeroSkinEvent() {SelectedHeroSkin = _skinSeletChecker.CurrentHeroSkin} );
            EventAggregator.Post(this, new ApplySelectedShieldSkinEvent() { SelectedShieldSkin = _skinSeletChecker.CurrentShieldSkin });
            
            ClickBackButton?.Invoke();
        }
    }
}