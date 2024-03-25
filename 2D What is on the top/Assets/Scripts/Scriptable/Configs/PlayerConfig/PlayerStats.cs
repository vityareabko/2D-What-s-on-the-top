using System;
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
    
    public int GetStatLevelByType(PlayerStatType type) => (int)CurrentPlayerStats[type];
    
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
    
    public (float, float?) GetStatCurrentAndNextLevelByType(PlayerStatType statType)
    {
        var currentLevel = BaseStateValues[statType] + UpgradeStatsLevels.First(t => t.Key == statType).Value.First(level => level.Level == CurrentPlayerStats[statType]).Value;
        
        float? nextLevel = null;
        
        if (CanUpgradeToNextLevel(statType))
            nextLevel = BaseStateValues[statType] + UpgradeStatsLevels.First(t => t.Key == statType).Value.First(level => level.Level == CurrentPlayerStats[statType] + 1).Value;
        
        return (currentStatLevelValue: currentLevel, nextStatLevelValue: nextLevel);
    }
    
    public (Sprite, int) GetStatPriceAndIconResourceByType(PlayerStatType statType)
    {
        var currentStatLevel = GetStatByType(statType);
        var priceToUpgrade = currentStatLevel.priceAmount;
        var ResourceSprite = currentStatLevel.ResourceIcon;
        

        return (IconResource: ResourceSprite, Amount: priceToUpgrade);

    }

    public float GetRunSpeed() => BaseStateValues[PlayerStatType.SpeedRun] + UpgradeStatsLevels[PlayerStatType.SpeedRun].First(level => level.Level == CurrentPlayerStats[PlayerStatType.SpeedRun]).Value;
    public float GetJumpPower() => BaseStateValues[PlayerStatType.ForceJump] + UpgradeStatsLevels[PlayerStatType.ForceJump].First(level => level.Level == CurrentPlayerStats[PlayerStatType.ForceJump]).Value;
    public float GetLuckValue() => BaseStateValues[PlayerStatType.Luck] + UpgradeStatsLevels[PlayerStatType.Luck].First(level => level.Level == CurrentPlayerStats[PlayerStatType.Luck]).Value;
    public float GetMaxStamina() => BaseStateValues[PlayerStatType.Stamina] + UpgradeStatsLevels[PlayerStatType.Stamina].First(level => level.Level == CurrentPlayerStats[PlayerStatType.Stamina]).Value;
    
    public float GetStaminaDrainRateRunning() => BaseStateValues[PlayerStatType.StaminaReductionForRunning] + UpgradeStatsLevels[PlayerStatType.StaminaReductionForRunning].First(level => level.Level == CurrentPlayerStats[PlayerStatType.StaminaReductionForRunning]).Value;
    public float GetStaminaDrainRateRoll() => BaseStateValues[PlayerStatType.StaminaReductionForRollUpward] + UpgradeStatsLevels[PlayerStatType.StaminaReductionForRollUpward].First(level => level.Level == CurrentPlayerStats[PlayerStatType.StaminaReductionForRollUpward]).Value;
    public float GetStaminaDrainRateJumping() => BaseStateValues[PlayerStatType.StaminaReductionForJump] + UpgradeStatsLevels[PlayerStatType.StaminaReductionForJump].First(level => level.Level == CurrentPlayerStats[PlayerStatType.StaminaReductionForJump]).Value;



    [Button("add Stamina LEvels - 30")]
    private void AddStaminaLevel()
    {
        float a = 3f;
        float b = 1.5f;
        
        for (var i = 1; i <= 30; i++)
        {
            // Рассчитываем новое значение стамины на уровне i
            var value = a * Math.Pow(b, i - 1); // Используем i - 1, так как стамина на первом уровне уже известна и равна a

            var level = new LevelStatUpgrade()
            {
                Level = (LevelStatType)i,
                Value = (int)Math.Round(value) // Округляем до ближайшего целого числа
            };

            UpgradeStatsLevels[PlayerStatType.Stamina].Add(level);
        }
    }

    [Button("Initialize Base Stats")]
    private void InitializeBaseStats()
    {
        foreach (PlayerStatType stat in Enum.GetValues(typeof(PlayerStatType)))
        {
            BaseStateValues[stat] = 0;
        }
    }
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
