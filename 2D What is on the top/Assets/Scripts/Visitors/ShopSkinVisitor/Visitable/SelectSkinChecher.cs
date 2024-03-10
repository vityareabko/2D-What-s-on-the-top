using MyNamespace.Scriptable.Configs.ShopSkins._1111;
using PersistentData;
using Scriptable.Configs.ShopSkins;

namespace ShopSkinVisitor.Visitable
{
    public class SelectSkinChecher : IShopSkinVisitor
    {
        private IPersistentPlayerData _persistentData;

        public bool IsSelect;

        public HeroSkinType CurrentHeroSkin;
        public ShieldSkinType CurrentShieldSkin;
        
        public SelectSkinChecher(IPersistentPlayerData persistentData)
        {
            _persistentData = persistentData;
            CurrentShieldSkin = persistentData.PlayerData.SelectedShieldSkin;
            CurrentHeroSkin = persistentData.PlayerData.SelectedHeroSkin;
        }

        public void Visit(HeroSkinItem heroSkinItem)
        {
            IsSelect = _persistentData.PlayerData.SelectedHeroSkin == heroSkinItem.Type;
            
            if (IsSelect)
                CurrentHeroSkin = _persistentData.PlayerData.SelectedHeroSkin;
        }

        public void Visit(ShieldSkinItem shieldSkinItem)
        {
            IsSelect = _persistentData.PlayerData.SelectedShieldSkin == shieldSkinItem.Type;
            
            if (IsSelect)
                CurrentShieldSkin = _persistentData.PlayerData.SelectedShieldSkin;
        }
    }
}