using System;
using System.Collections.Generic;
using System.Linq;
using MyNamespace.Scriptable.Configs.ShopSkins._1111;
using Scriptable.Configs.ShopSkins;
using UnityEngine;


[CreateAssetMenu(fileName = "ShopSkinDB", menuName = "Databases/ShopSkinDB")]
public class ShopSkinDB : ScriptableObject
{
    [SerializeField] private List<HeroSkinItem> _heroSkinItems;
    [SerializeField] private List<ShieldSkinItem> _shieldSkinItems;

    private void OnValidate()
    {
        if (_heroSkinItems.GroupBy(t => t.Type).Any(d => d.Count() > 1))
            Debug.LogError($"[ShopSkinDB] Обнаружены дубликаты ", this);
        
        if (_shieldSkinItems.GroupBy(t => t.Type).Any(d => d.Count() > 1))
            Debug.LogError($"[ShopSkinDB] Обнаружены дубликаты ", this);
    }
    
    public IEnumerable<HeroSkinItem> HeroSkinItems => _heroSkinItems;
    public IEnumerable<ShieldSkinItem> ShieldSkinItems => _shieldSkinItems;


}
