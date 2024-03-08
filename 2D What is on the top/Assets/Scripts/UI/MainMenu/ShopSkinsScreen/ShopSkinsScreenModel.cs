using System.Collections.Generic;
using MyNamespace.Services.StorageService.SelectorSkin;
using UI.MVP;
using UnlockerSkins;
using WalletResources;

namespace UI.MainMenu.ShopSkinsScreen
{
    public interface IShopSkinsScreenModel : IModel
    {
        public IEnumerable<SkinItemConfig> GetSkins();
        public void SelectSkin(ShopSkinType type);
        public bool TryBuySkin(SkinItemConfig item);
    }

    public class ShopSkinsScreenModel : IShopSkinsScreenModel
    {
        
        private ShopSkinDB _shopSkinDB;
        private ISelectSkin _selectSkin;
        private IUnlockerSkin _unlockerSkins;
        private IWalletResource _walletResource;

        public ShopSkinsScreenModel(ShopSkinDB shopSkinDB, ISelectSkin selectSkin, IUnlockerSkin unlockerSkins, IWalletResource walletResource)
        {
            _shopSkinDB = shopSkinDB;
            _selectSkin = selectSkin;
            _unlockerSkins = unlockerSkins;
            _walletResource = walletResource;
        }

        public IEnumerable<SkinItemConfig> GetSkins() => _shopSkinDB.Skins;

        public void SelectSkin(ShopSkinType type) => _selectSkin.Select(type);
        
        public bool TryBuySkin(SkinItemConfig item)
        {
            if (CanAffordSkin(item) == false)
                return false;
            
            _unlockerSkins.Unlock(item.Type);
            _selectSkin.Select(item.Type);
            return true;
        }

        private bool CanAffordSkin(SkinItemConfig item) => _walletResource.Spend(item.ResourceTypeByBuySkin, item.PriceCoin);
    }
}