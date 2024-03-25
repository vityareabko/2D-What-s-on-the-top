using UnityEngine;

[System.Serializable]
public class LevelStatUpgrade
{
    [field: SerializeField] public LevelStatType Level { get; set; } = LevelStatType.Level1;
    [field: SerializeField] public ResourceTypes ResourceTypes { get; set; } = ResourceTypes.Coin;
    [field: SerializeField] public float Value { get; set; } = 0f;
    [field: SerializeField] public int priceAmount { get; set; } = 0;

    public Sprite ResourceIcon => ResourceService.LoadSpriteByType(ResourceTypes);
    
}
