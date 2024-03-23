using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerStats", menuName = "UpgradeStatsLevels/PlayerStats")]
public class PlayerStats : SerializedScriptableObject
{
    // static stats
    [field: SerializeField] public float RollVerticalSpeed { get; private set; } = 8f;
    [field: SerializeField] public float WalkVerticalSpeed { get; private set; } = 0.4f;
    [field: SerializeField] public float StaminaDrainRateWalking { get; private set; } = 75f;
    [field: SerializeField] public float StaminaDrainObstacleCollision { get; private set; } = 0.1f;
    
    
    // dynamic stats 
    public Dictionary<PlayerStatType, float> BaseStateValues = new (); 
    public Dictionary<PlayerStatType, List<LevelStatUpgrade>> UpgradeStatsLevels = new();  
    public Dictionary<PlayerStatType, LevelStatType> CurrentPlayerStats = new ();
    
    public void InitializeDataFromLoad(Dictionary<PlayerStatType, LevelStatType> currentStats) => CurrentPlayerStats = currentStats;
    
    public LevelStatUpgrade GetStatByType(PlayerStatType statType) => UpgradeStatsLevels.First(t => t.Key == statType).Value.First(level => level.Level == CurrentPlayerStats[statType]);
    
    public bool CanUpgradeToNextLevel(PlayerStatType statType)
    {
        var currentStatLevel = CurrentPlayerStats.First(t => t.Key == statType).Value;
        var nextLevelKey = currentStatLevel + 1;
      
        if ((int)nextLevelKey < UpgradeStatsLevels[statType].Count)
            return true;

        return false;
    }

    public void UpgradeStatLevel(PlayerStatType statType)
    {
        var nextLevelKey = CurrentPlayerStats[statType] + 1;
        CurrentPlayerStats[statType] = nextLevelKey;
    }

    public int GetStatLevelByType(PlayerStatType type) => (int)CurrentPlayerStats[type];

    public (float, float?) GetStatCurrentAndNextLevelByType(PlayerStatType statType)
    {
        var currentLevel = UpgradeStatsLevels.First(t => t.Key == statType).Value.First(level => level.Level == CurrentPlayerStats[statType]);
        
        LevelStatUpgrade nextLevel = null;
        
        if (CanUpgradeToNextLevel(statType))
            nextLevel = UpgradeStatsLevels.First(t => t.Key == statType).Value.First(level => level.Level == CurrentPlayerStats[statType] + 1);
        
        return (currentStatLevelValue: currentLevel.Value, nextStatLevelValue: nextLevel?.Value);
    }
    
    public (Sprite, int) GetStatPriceAndIconResourceByType(PlayerStatType statType)
    {
        var currentStatLevel = GetStatByType(statType);
        var priceToUpgrade = currentStatLevel.priceAmount;
        var ResourceSprite = currentStatLevel.ResourceIcon;
        
        Debug.Log(statType);

        return (IconResource: ResourceSprite, Amount: priceToUpgrade);

    }

    public float GetRunSpeed() => UpgradeStatsLevels[PlayerStatType.SpeedRun].First(level => level.Level == CurrentPlayerStats[PlayerStatType.SpeedRun]).Value;
    public float GetJumpPower() => UpgradeStatsLevels[PlayerStatType.ForceJump].First(level => level.Level == CurrentPlayerStats[PlayerStatType.ForceJump]).Value;
    public float GetLuckValue() => UpgradeStatsLevels[PlayerStatType.Luck].First(level => level.Level == CurrentPlayerStats[PlayerStatType.Luck]).Value;
    public float GetMaxStamina() => UpgradeStatsLevels[PlayerStatType.Stamina].First(level => level.Level == CurrentPlayerStats[PlayerStatType.Stamina]).Value;
    
    public float GetStaminaDrainRateRunning() => UpgradeStatsLevels[PlayerStatType.StaminaReductionForRunning].First(level => level.Level == CurrentPlayerStats[PlayerStatType.StaminaReductionForRunning]).Value;
    public float GetStaminaDrainRateRoll() => UpgradeStatsLevels[PlayerStatType.StaminaReductionForRollUpward].First(level => level.Level == CurrentPlayerStats[PlayerStatType.StaminaReductionForRollUpward]).Value;
    public float GetStaminaDrainRateJumping() => UpgradeStatsLevels[PlayerStatType.StaminaReductionForJump].First(level => level.Level == CurrentPlayerStats[PlayerStatType.StaminaReductionForJump]).Value;
}


/// 
///  - вынсоливость - нет макс. и посколько нет максимального объеима прокачка будет медленой 
///  - скорость бега - 3 - 6
///  - сила прыжка - 5 - 10
///  - удача - 100 - 1000
///  - снижения траты выносливости за бег
///  - снижения траты выносливости за прыжок
///  - снижения траты выносливости за кувырок
///
