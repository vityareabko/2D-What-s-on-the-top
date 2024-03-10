using MyNamespace.Scriptable.Configs.ShopSkins._1111;
using ShopSkinVisitor.Visitable;
using UI.MVP;

using WalletResources;

namespace UI.MainMenu.ShopSkinsScreen
{
    public interface IShopSkinsScreenModel : IModel
    {
        public void SelectSkin(SkinItem item);
        public bool TryBuySkin(SkinItem item);
    }

    public class ShopSkinsScreenModel : IShopSkinsScreenModel
    {
        private IWalletResource _walletResource;
        
        private OpenShopSkin _openShopSkin;
        private SelectShopSkin _selectSkin;
        
        public ShopSkinsScreenModel(IWalletResource walletResource, OpenShopSkin openShopSkin, SelectShopSkin selectSkin)
        {
            _walletResource = walletResource;
            _openShopSkin = openShopSkin;
            _selectSkin = selectSkin;
        }

        public void SelectSkin(SkinItem item) => item.Accept(_selectSkin);
        
        public bool TryBuySkin(SkinItem item)
        {
            if (_walletResource.HasEnoughResourceAmount(item.ResourceTypeByBuySkin, item.PriceCoin) == false)
                return false;
            
            item.Accept(_openShopSkin);
            _walletResource.Spend(item.ResourceTypeByBuySkin, item.PriceCoin);
            item.Accept(_selectSkin);
            
            return true;
        }
    }
}