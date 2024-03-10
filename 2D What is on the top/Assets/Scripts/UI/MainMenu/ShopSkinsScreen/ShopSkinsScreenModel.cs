using MyNamespace.Scriptable.Configs.ShopSkins._1111;
using PersistentData;
using Scriptable.Configs.ShopSkins.@base;
using ShopSkinVisitor.Visitable;
using UI.MVP;

// using WalletResources;

namespace UI.MainMenu.ShopSkinsScreen
{
    public interface IShopSkinsScreenModel : IModel
    {
        public void SelectSkin(SkinItem item);
        public bool TryBuySkin(SkinItem item);
    }

    public class ShopSkinsScreenModel : IShopSkinsScreenModel
    {
        // private IWalletResource _walletResource;
        
        private OpenShopSkin _openShopSkin;
        private SelectShopSkin _selectSkin;
        private IPersistentResourceData _persistentResourceData;
        
        // public ShopSkinsScreenModel(IWalletResource walletResource, OpenShopSkin openShopSkin, SelectShopSkin selectSkin)
        public ShopSkinsScreenModel(IPersistentResourceData walletResource, OpenShopSkin openShopSkin, SelectShopSkin selectSkin)
        {
            _persistentResourceData = walletResource;
            _openShopSkin = openShopSkin;
            _selectSkin = selectSkin;
        }

        public void SelectSkin(SkinItem item) => item.Accept(_selectSkin);
        
        public bool TryBuySkin(SkinItem item)
        {
            if (_persistentResourceData.ResourcesJsonData.HasEnoughResourceAmount(item.ResourceTypeByBuySkin, item.PriceCoin) == false)
                return false;
            
            item.Accept(_openShopSkin);
            _persistentResourceData.ResourcesJsonData.Spend(item.ResourceTypeByBuySkin, item.PriceCoin);
            _persistentResourceData.SaveData();
            item.Accept(_selectSkin);
            
            return true;
        }
    }
}