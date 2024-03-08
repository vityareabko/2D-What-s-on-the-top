using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "SkinItem", menuName = "Config/ShopSkinItem")]
public class SkinItemConfig : ScriptableObject
{
    [field: SerializeField] public ShopSkinType Type { get; private set; }
    [field: SerializeField] public Sprite ShopIcon { get; private set; }

    [field: SerializeField] public ResourceTypes ResourceTypeByBuySkin { get; private set; } = ResourceTypes.Coin;
    [field: SerializeField] public int PriceCoin { get; private set; }
}
