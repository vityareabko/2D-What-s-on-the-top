using UI.MainMenu.ShopSkinItemPanel;
using UnityEngine;


[CreateAssetMenu(fileName = "ShopSkinFactory", menuName = "Factory/ShopSkinFactory")]
public class ShopSkinFactory : ScriptableObject
{
    // [field: SerializeField] public ShopSkinDB _shopSkinDB;

    [SerializeField] private ShopSkinItemView _itemSkinPrefab;
    
    public ShopSkinItemView Get(SkinItemConfig config, Transform parentTransform)
    {
        var instance = Instantiate(_itemSkinPrefab, parentTransform);
        instance.Initialize(config);
        
        return null;
    }
}
