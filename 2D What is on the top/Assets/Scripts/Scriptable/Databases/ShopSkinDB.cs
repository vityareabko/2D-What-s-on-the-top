using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "ShopSkinDB", menuName = "Databases/ShopSkinDB")]
public class ShopSkinDB : ScriptableObject
{
    [field: SerializeField] public List<SkinItemConfig> HeroSkins { get; private set; } 
    
    [field: SerializeField] public List<SkinItemConfig> ShieldSkins { get; private set; } 
    
    // Todo: OnValidate - проверка на повторение типов сделать
    private void OnValidate()
    {
        if (HeroSkins.GroupBy(t => t.Type).Any(d => d.Count() > 1))
            Debug.LogError($"[ShopSkinDB] Обнаружены дубликаты ", this);
        
        if (ShieldSkins.GroupBy(t => t.Type).Any(d => d.Count() > 1))
            Debug.LogError($"[ShopSkinDB] Обнаружены дубликаты ", this);
    }
}
