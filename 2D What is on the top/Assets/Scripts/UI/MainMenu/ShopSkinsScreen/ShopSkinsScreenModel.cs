using System.Collections.Generic;
using MyNamespace.Services.StorageService.SelectorSkin;
using UI.MVP;
using UnlockerSkins;
using WalletResources;

namespace UI.MainMenu.ShopSkinsScreen
{
    public interface IShopSkinsScreenModel : IModel
    {
        public void SelectSkin(ShopSkinType type, ShopSkinTabType tabType);
        public bool TryBuySkin(SkinItemConfig item, ShopSkinTabType tabType);
    }

    public class ShopSkinsScreenModel : IShopSkinsScreenModel
    {
        
        private ISelectSkin _selectSkin;
        private IUnlockerSkin _unlockerSkins;
        private IWalletResource _walletResource;

        public ShopSkinsScreenModel(ISelectSkin selectSkin, IUnlockerSkin unlockerSkins, IWalletResource walletResource)
        {
            _selectSkin = selectSkin;
            _unlockerSkins = unlockerSkins;
            _walletResource = walletResource;
        }
        
        public void SelectSkin(ShopSkinType type, ShopSkinTabType tabType) => _selectSkin.Select(type, tabType);
        
        public bool TryBuySkin(SkinItemConfig item, ShopSkinTabType tabType)
        {
            if (CanAffordSkin(item) == false)
                return false;
            
            _unlockerSkins.Unlock(item.Type);
            _selectSkin.Select(item.Type, tabType);
            return true;
        }

        private bool CanAffordSkin(SkinItemConfig item) => _walletResource.Spend(item.ResourceTypeByBuySkin, item.PriceCoin);
    }
}