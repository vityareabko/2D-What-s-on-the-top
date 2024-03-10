using MyNamespace.Scriptable.Configs.ShopSkins._1111;
using Scriptable.Configs.ShopSkins;

namespace ShopSkinVisitor.Visitable
{
    public class ClickSkinItemView : IShopSkinVisitor
    {
        public void Visit(HeroSkinItem heroSkinItem) =>
            EventAggregator.Post(this, new TryOnSkinEvent() { selectedHeroSkinTypeSkin = heroSkinItem.Type });
        
        public void Visit(ShieldSkinItem shieldSkinItem) =>
            EventAggregator.Post(this, new TryOnShieldSkinEvent() { shieldSkin = shieldSkinItem.Type });
    }
}