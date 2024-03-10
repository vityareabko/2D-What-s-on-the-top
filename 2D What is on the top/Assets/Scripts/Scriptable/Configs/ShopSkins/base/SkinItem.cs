using ShopSkinVisitor.Visitable;
using UnityEngine;

namespace Scriptable.Configs.ShopSkins.@base
{
    public abstract class SkinItem : ScriptableObject
    {
        [field: SerializeField] public Sprite ShopIcon { get; private set; }
        [field: SerializeField] public ResourceTypes ResourceTypeByBuySkin { get; private set; } = ResourceTypes.Coin;
        [field: SerializeField] public int PriceCoin { get; private set; }

        public abstract void Accept(IShopSkinVisitor visitor);
    }
}