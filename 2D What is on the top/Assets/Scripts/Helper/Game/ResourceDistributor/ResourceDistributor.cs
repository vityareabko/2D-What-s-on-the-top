using System;
using System.Collections.Generic;

public static class ResourceDistributor
{
    private static readonly Dictionary<ResourceTypes, ResourceCategory> _resourceCategoryMap = new()
    {
        { ResourceTypes.Coin, ResourceCategory.Common},
        { ResourceTypes.Gem, ResourceCategory.Common},
        { ResourceTypes.NormalAlichea, ResourceCategory.Normal},
        { ResourceTypes.NormalApple, ResourceCategory.Normal},
        { ResourceTypes.NormalCherry, ResourceCategory.Normal},
        { ResourceTypes.NormalDragonBarry, ResourceCategory.Normal},
        { ResourceTypes.NormalPumpkin, ResourceCategory.Normal},
        { ResourceTypes.NormalStrawberry, ResourceCategory.Normal},
        { ResourceTypes.RareBucket, ResourceCategory.Rare},
        { ResourceTypes.RareDagger, ResourceCategory.Rare},
        { ResourceTypes.RareMuteWater, ResourceCategory.Rare},
        { ResourceTypes.EpicDivineFire, ResourceCategory.Epic},
        { ResourceTypes.EpicMedal, ResourceCategory.Epic},
        { ResourceTypes.EpicShield, ResourceCategory.Epic},
        { ResourceTypes.LegendaryBranch, ResourceCategory.Legendary},
        { ResourceTypes.LegendaryLeaf, ResourceCategory.Legendary},
        { ResourceTypes.LegendaryStone, ResourceCategory.Legendary}
        
    };

    public static ResourceCategory GetCategoryResourceByResourceType(ResourceTypes type)
    {
        if (_resourceCategoryMap.TryGetValue(type, out var category))
            return category; 
        
        throw new ArgumentException("Resource type does not have an associated category", nameof(type));
    }
}
