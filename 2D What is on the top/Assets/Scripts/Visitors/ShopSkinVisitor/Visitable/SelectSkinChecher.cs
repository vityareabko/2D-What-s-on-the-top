using MyNamespace.Scriptable.Configs.ShopSkins._1111;
using PersistentPlayerData;
using Scriptable.Configs.ShopSkins;

namespace ShopSkinVisitor.Visitable
{
    public class SelectSkinChecher : IShopSkinVisitor
    {
        private IPersistentData _persistentData;

        public bool IsSelect;

        public HeroSkinType CurrentHeroSkin;
        public ShieldSkinType CurrentShieldSkin;
        
        public SelectSkinChecher(IPersistentData persistentData)
        {
            _persistentData = persistentData;
            CurrentShieldSkin = persistentData.PlayerData.SelectedShieldSkin;
            CurrentHeroSkin = persistentData.PlayerData.SelectedHeroSkin;
        }

        public void Visit(HeroSkinItem heroSkinItem) => IsSelect = _persistentData.PlayerData.SelectedHeroSkin == heroSkinItem.Type;

        public void Visit(ShieldSkinItem shieldSkinItem) => IsSelect = _persistentData.PlayerData.SelectedShieldSkin == shieldSkinItem.Type;
    }
}