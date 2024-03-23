
using System;
using System.Collections.Generic;
using UnityEngine;
using VHierarchy.Libs;

public static class ResourceService
{
    private static Dictionary<string, UnityEngine.Object> resourceCache = new();

    public static void CleanCahs() => resourceCache.Clear();

    public static Sprite LoadSpriteByType(ResourceTypes type)
    {
        var path = "Sprites";
        switch (type)
        {
            case ResourceTypes.Coin:
                path = path.CombinePath("Coin");
                break;
            case ResourceTypes.Gem:
                path = path.CombinePath("Gem");
                break;
            case ResourceTypes.NormalAlichea:
                path = path.CombinePath("normalAlichea");
                break;
            case ResourceTypes.NormalApple:
                path = path.CombinePath("normalApple");
                break;
            case ResourceTypes.NormalCherry:
                path = path.CombinePath("normalCherry");
                break;
            case ResourceTypes.NormalDragonBarry:
                path = path.CombinePath("normalDragonBarry");
                break;
            case ResourceTypes.NormalPumpkin:
                path = path.CombinePath("normalPumpkin");
                break;
            case ResourceTypes.NormalStrawberry:
                path = path.CombinePath("normalStrawberry");
                break;
            case ResourceTypes.RareBucket:
                path = path.CombinePath("rareBucket");
                break;
            case ResourceTypes.RareDagger:
                path = path.CombinePath("rareDagger");
                break;
            case ResourceTypes.RareMuteWater:
                path = path.CombinePath("rareMuteWater");
                break;
            case ResourceTypes.EpicDivineFire:
                path = path.CombinePath("epicDivineFire");
                break;
            case ResourceTypes.EpicMedal:
                path = path.CombinePath("epicMedal");
                break;
            case ResourceTypes.EpicShield:
                path = path.CombinePath("epicShield");
                break;
            case ResourceTypes.LegendaryBranch:
                path = path.CombinePath("legendaryBranch");
                break;
            case ResourceTypes.LegendaryLeaf:
                path = path.CombinePath("legendaryLeaf");
                break;
            case ResourceTypes.LegendaryStone:
                path = path.CombinePath("legendaryStone");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        
        Debug.Log(path);
        return Load<Sprite>(path);
    }
    
    
    private static T Load<T>(string resourcePath) where T : UnityEngine.Object
    {
        if (resourceCache.TryGetValue(resourcePath, out UnityEngine.Object cachedResource) && cachedResource is T)
            return cachedResource as T;

        T resource = Resources.Load<T>(resourcePath);

        if (resource != null)
            resourceCache[resourcePath] = resource;

        return resource;
    }
}