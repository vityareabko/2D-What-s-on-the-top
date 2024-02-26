using System;
using System.Collections.Generic;
using System.Linq;
using Obstacles;
using Scriptable.Datas.FallResources;
using Sirenix.OdinInspector;
using Systems.ResourcesLoaderSystem;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu (fileName = "ConfigLevel", menuName = "Config/LevelConfig")]
public class LevelConfig : SerializedScriptableObject
{
    [field: SerializeField,  OnValueChanged("DeterminePathToResourcesByLevelType")] public LevelType Type { get; private set; }
    [SerializeField, ReadOnly] private ResourceID _pathToResourcesByLevelType;
        
    [FoldoutGroup("LevelDatas")] [field: SerializeField] public LevelConfigDatas LevelDatas;
    
    [FoldoutGroup("Available To Spawn")] public Dictionary<ResourceCategory, List<FallingResourceConfig>> ResourcesByCategory;
    [FoldoutGroup("Available To Spawn")] public Dictionary<ObstacleCategory, List<FallObstacleConfig>> ObstaclesByCategory;

    public ResourceID PathToResourcesByLevelType { get => _pathToResourcesByLevelType; }

    public void OnValidate()
    {
        
        ValidateResources();
        ValidateObstacles();
    }

    [Button("Initialize Dictionaries")]
    private void InitializeDictionaries()
    {
        InitializeObstacleCategories();
        InitializeResourcesCategories();
    }

    private void ValidateObstacles()
    {
        if (ObstaclesByCategory == null)
            return;
        
        foreach (var category in ObstaclesByCategory.Keys.ToList())
        {
            var obstacles = ObstaclesByCategory[category];
            
            if (obstacles.GroupBy(x => x.Type).Any(g => g.Count() > 1))
                Debug.LogError($"[{category} Resource] Обнаружены дубликаты.");
            
            foreach (var obstacle in obstacles)
            {
                if (obstacle.CategoryType != category) 
                {
                    Debug.LogError($"[LevelConfig] Обнаружен ресурс другой категории в списке '{category} Resources'.");
                    break; 
                }
            }
        }
    }

    private void ValidateResources()
    {
        if (ResourcesByCategory == null)
            return;
        
        foreach (var category in ResourcesByCategory.Keys.ToList())
        {
            var resources = ResourcesByCategory[category];
            
            if (resources.GroupBy(x => x.Type).Any(g => g.Count() > 1))
                Debug.LogError($"[{category} Resource] Обнаружены дубликаты.");
            
            foreach (var resource in resources)
            {
                if (resource.CategoryType != category) 
                {
                    Debug.LogError($"[LevelConfig] Обнаружен ресурс другой категории в списке '{category} Resources'.");
                    break; 
                }
            }
        }
    }
    
    private void InitializeObstacleCategories()
    {
        if (ObstaclesByCategory == null)
        {
            ObstaclesByCategory = new Dictionary<ObstacleCategory, List<FallObstacleConfig>>();
            
            foreach (ObstacleCategory category in System.Enum.GetValues(typeof(ObstacleCategory)))
            {
                ObstaclesByCategory[category] = new List<FallObstacleConfig>();
            }
        }
    }
    
    private void InitializeResourcesCategories()
    {
        if (ResourcesByCategory == null)
        {
            ResourcesByCategory = new Dictionary<ResourceCategory, List<FallingResourceConfig>>();
            
            foreach (ResourceCategory category in System.Enum.GetValues(typeof(ResourceCategory)))
            {
                ResourcesByCategory[category] = new List<FallingResourceConfig>();
            }
        }
    }
    
    private void DeterminePathToResourcesByLevelType()
    {
        switch (Type)
        {
            case LevelType.Level1:
                _pathToResourcesByLevelType = ResourceID.Level1Prefab;
                break;
            case LevelType.TestLevel:
                _pathToResourcesByLevelType = ResourceID.TestLevelPrefab;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

