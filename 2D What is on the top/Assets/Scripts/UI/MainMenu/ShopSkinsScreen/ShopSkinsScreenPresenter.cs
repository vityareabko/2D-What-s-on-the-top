using System;
using System.Collections.Generic;
using ModestTree;
using UI.MainMenu.ShopSkinItemPanel;
using UI.MVP;

namespace UI.MainMenu.ShopSkinsScreen
{
    public interface IShopSkinsScreenPresenter : IPresenter<IShopSkinsScreenModel, IShopSkinsScreenView>
    {
        public event Action ClickBuyButton; 
        public event Action ClickSelectButton; 
        public void OnClickBuyButton();
        public void OnClickSelectButton();
    }

    public class ShopSkinsScreenPresenter : IShopSkinsScreenPresenter
    {
        public event Action ClickBuyButton;
        public event Action ClickSelectButton;
        
        public IShopSkinsScreenModel Model { get; private set; }
        public IShopSkinsScreenView View { get; private set; }

        private bool _isInit;
        
        // private List<ShopSkinItemView> _shopItems = new();
        private ShopSkinFactory _skinFactory;
        private ShopSkinDB _shopSkinDB;
        
        private ShopSkinsScreenPresenter(IShopSkinsScreenModel model, IShopSkinsScreenView view, ShopSkinDB shopSkinDB, ShopSkinFactory shopSkinFactory)
        {
            Model = model;
            View = view;

            _shopSkinDB = shopSkinDB;
            _skinFactory = shopSkinFactory;
            Init();
        }

        public void Init()
        {
            if (_isInit)
                return;

            _isInit = true;
            GenerateShopContent();
            View.InitPresentor(this);
        }

        public void Show() => View.Show();

        public void Hide() => View.Hide();

        private void GenerateShopContent()
        {
            foreach (var skinItemConfig in _shopSkinDB.Skins)
            {
                var instance = _skinFactory.Get(skinItemConfig, View.ContentTransform);
                // _shopItems.Add(instance);
                
                // TODO : - нужно забиндить все и про чекать если все создается 
            }
        }

        public void OnClickBuyButton() => ClickBuyButton?.Invoke();

        public void OnClickSelectButton() => ClickSelectButton?.Invoke();
    }
}