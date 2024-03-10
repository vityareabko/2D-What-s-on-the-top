using MyNamespace.Scriptable.Configs.ShopSkins._1111;
using Scriptable.Configs.ShopSkins;

namespace ShopSkinVisitor.Visitable
{
    public interface IShopSkinVisitor
    {
        public void Visit(HeroSkinItem heroSkinItem);
        public void Visit(ShieldSkinItem shieldSkinItem);
    }
}