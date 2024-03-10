using ShopSkinVisitor.Visitable;
using UnityEngine;

namespace MyNamespace.Scriptable.Configs.ShopSkins._1111
{
    [CreateAssetMenu(fileName = "HeroSkinItem", menuName = "Shop/HeroSkinItem")]
    public class HeroSkinItem : SkinItem, IShopSkinVisitable
    {
        [field: SerializeField] public HeroSkinType Type { get; private set; }
        
        public override void Accept(IShopSkinVisitor visitor) => visitor.Visit(this);
        
    }
}