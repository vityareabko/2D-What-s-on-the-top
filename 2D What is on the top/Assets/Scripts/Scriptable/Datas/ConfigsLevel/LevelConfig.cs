
using System.Collections.Generic;
using System.Linq;
using Obstacles;
using Scriptable.Datas.FallResources;
using Sirenix.OdinInspector;
using UnityEngine;


[CreateAssetMenu (fileName = "ConfigLevel", menuName = "Config/LevelConfig")]
public class LevelConfig : SerializedScriptableObject
{
    [field: SerializeField] public int MaxHeightLevel { get; private set; }
    [field: SerializeField] public float MinTimeSpawObstacle { get; private set; }
    [field: SerializeField] public float StartTimeSpawmObstacle { get; private set; }

    [field: SerializeField] public float StartTimeSpawnResource { get; private set; }
    [field: SerializeField] public float MinTimeSpawnResource { get; private set; }
    
    [TabGroup("Resources")]
    public Dictionary<ResourceCategory, List<FallingResourceConfig>> ResourcesByCategory;

    public void OnValidate()
    {
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
}


/// <summary>
///
/// Как я хочу чтобы было:
/// я хочу ограничить в доступе рессурсов на разных уровнях (я зочу чтобы к примеру на первом уровне выпадала какой-то список рессурсов а не все подряд)
///
/// 
/// Что у меня есть:
/// у меня в уровне падают все типы рессурсов которые есть (там чутка зависит от вероятности, но в целом каждый рессурс может выпасть на уровне)
/// у менч есть класс который создает словарь с константы которые отвечаюь за кол-во типов в масиве из категории чем нижее по цености категория тем больше занимает масив этот тип
/// у меня есть фабрика в котором я передаю тип который выпал с учетов вероятности, и мне спавнит а также возращает рандомный рессуры надлжежащей категроии (если нужно будет ку-то прокидывать)
/// 
/// 
/// Что я могу сделать :
/// мне нужно объяевить в LevelConfig List<FallResourcesConfi>() - где будет содержатся рессурсы которые могут падать на этом уровне
/// потом мне нужно передать этот список из конфига в класс ResourceBalancer - где уже будет создаватся масив из типов которые в этом List находится а остальные типы нужно будет убирать
/// после этого нужно передать также этот List из конфига фабрике чтобы она знала какие типы доступны для выдачи
///
/// 
/// </summary>