using MyNamespace.Scriptable.Configs.ShopSkins._1111;
using Scriptable.Configs.ShopSkins.@base;
using ShopSkinVisitor.Visitable;
using UnityEngine;

namespace Scriptable.Configs.ShopSkins
{
    [CreateAssetMenu(fileName = "ShieldSkinItem", menuName = "Shop/ShieldSkinItem")]
    public class ShieldSkinItem : SkinItem, IShopSkinVisitable
    {
        [field: SerializeField] public ShieldSkinType Type { get; private set; }
        

        public override void Accept(IShopSkinVisitor visitor) => visitor.Visit(this);
    }
}