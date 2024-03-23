using UnityEngine;
using UnityEngine.Serialization;


[System.Serializable]
public class LevelStatUpgrade
{
    [field: SerializeField] public LevelStatType Level = LevelStatType.Level1;
    [field: SerializeField] public ResourceTypes ResourceTypes = ResourceTypes.Coin;
    [FormerlySerializedAs("ImproveValue")] [field: SerializeField] public float Value = 0f;
    [field: SerializeField] public int priceAmount = 0;

    public Sprite ResourceIcon => ResourceService.LoadSpriteByType(ResourceTypes);
    
}
