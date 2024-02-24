using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "Databases/LevelsDatabase")]
public class LevelDatabases : SerializedScriptableObject
{
    public Dictionary<LevelType, LevelConfig> LevelConfigs;

    [Button("Initialize Dictionary")]
    private void InitializeDictionary(List<LevelConfig> levelConfigs)
    {
        foreach (var levelConfig in levelConfigs)
        {
            if (LevelConfigs.ContainsKey(levelConfig.Type) == false)
                LevelConfigs[levelConfig.Type] = levelConfig;
        }
    }
}


/// <summary>
///
/// Как я хочу чтобы было:
/// я хочу чтобы я нажымал на кнопку и играть и переходил на сцену выбор карту и в зависимости от выбора карты переходил на сцену где будет спавнится сама карта игрок и другое
/// 
/// Что у меня есть:
/// у меня есть конфиг уровня в котором я указываю какие-то базовые параметри для динамического спавна объектов по мере прохождения игрока.
/// досутупнын рессурсы для спавна на уровне
/// и доступные препятсвия которые будет спавнится на уровне
///
/// 
/// Что я могу сделать:
/// я могу сделать БД для уровней в котором будут хранится все уровни
/// за биндит эту БД в глобальный контайнер
/// в зависимости от выбраного уровня передать нужный конфиг уровня в сцену 
/// 
/// </summary>