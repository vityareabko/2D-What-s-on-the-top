using MyNamespace.Scriptable.Configs.ShopSkins._1111;
using PersistentPlayerData;
using Scriptable.Configs.ShopSkins;

namespace ShopSkinVisitor.Visitable
{
    public class OpenShopSkin : IShopSkinVisitor
    {
        private IPersistentData _persistentData;

        public OpenShopSkin(IPersistentData persistentData) => _persistentData = persistentData;

        public void Visit(HeroSkinItem heroSkinItem) => _persistentData.PlayerData.OpenHeroSkin(heroSkinItem.Type);
            
        public void Visit(ShieldSkinItem shieldSkinItem) => _persistentData.PlayerData.OpenShieldSkin(shieldSkinItem.Type);
    }
}