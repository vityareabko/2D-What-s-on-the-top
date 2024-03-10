using MyNamespace.Scriptable.Configs.ShopSkins._1111;
using PersistentPlayerData;
using Scriptable.Configs.ShopSkins;

namespace ShopSkinVisitor.Visitable
{
    public class SelectShopSkin : IShopSkinVisitor
    {
        private IPersistentData _persistentData;

        public SelectShopSkin(IPersistentData persistentData) => _persistentData = persistentData;

        public void Visit(HeroSkinItem heroSkinItem) => _persistentData.PlayerData.SelectedHeroSkin = heroSkinItem.Type;

        public void Visit(ShieldSkinItem shieldSkinItem) => _persistentData.PlayerData.SelectedShieldSkin = shieldSkinItem.Type;
    }
}