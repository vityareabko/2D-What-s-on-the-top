using System;
using MyNamespace.Scriptable.Configs.ShopSkins._1111;
using Scriptable.Configs.ShopSkins;
using ShopSkinVisitor;
using ShopSkinVisitor.Visitable;
using UI.MainMenu.ShopSkinItemPanel;
using UnityEngine;


[CreateAssetMenu(fileName = "ShopSkinFactory", menuName = "Factory/ShopSkinFactory")]
public class ShopSkinFactory : ScriptableObject
{
    [SerializeField] private ShopSkinItemView _heroSkinItemView;
    [SerializeField] private ShopSkinItemView _shieldSkinItemView;

    public ShopSkinItemView Get(SkinItem skinItem, Transform parent)
    {

        var visitor = new ShopItemVisitor(_heroSkinItemView, _shieldSkinItemView);
        skinItem.Accept(visitor);

        var instance = Instantiate(visitor.Prefab, parent);
        instance.Initialize(skinItem);

        return instance;
    }
    
    private class ShopItemVisitor : IShopSkinVisitor
    {
        private ShopSkinItemView _heroSkinItemView;
        private ShopSkinItemView _shieldSkinItemView;

        public ShopSkinItemView Prefab;
        
        public ShopItemVisitor(ShopSkinItemView heroSkinItemView, ShopSkinItemView shieldSkinItemView)
        {
            _heroSkinItemView = heroSkinItemView;
            _shieldSkinItemView = shieldSkinItemView;
        }
        
        public void Visit(HeroSkinItem heroSkinItem) => Prefab = _heroSkinItemView;

        public void Visit(ShieldSkinItem shieldSkinItem) => Prefab = _shieldSkinItemView;
    }

}
