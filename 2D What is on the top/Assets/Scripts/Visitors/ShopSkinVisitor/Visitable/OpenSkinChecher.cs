using System.Linq;
using MyNamespace.Scriptable.Configs.ShopSkins._1111;
using PersistentPlayerData;
using Scriptable.Configs.ShopSkins;

namespace ShopSkinVisitor.Visitable
{
    public class OpenSkinChecher : IShopSkinVisitor
    {
        private IPersistentData _persistentData;

        public bool IsOpen;
        
        public OpenSkinChecher(IPersistentData persistentData) => _persistentData = persistentData;

        public void Visit(HeroSkinItem heroSkinItem) => IsOpen = _persistentData.PlayerData.AvailableHeroSkins.Contains(heroSkinItem.Type);

        public void Visit(ShieldSkinItem shieldSkinItem) => IsOpen = _persistentData.PlayerData.AvailableShieldSkins.Contains(shieldSkinItem.Type);
    }
}