using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Obstacles
{
    [CreateAssetMenu(fileName = "LevelsDB", menuName = "Databases/LevelsDB")]
    public class LevelsDB : ScriptableObject
    {
        [field: SerializeField] public List<LevelConfig> _levels { get; private set; }

        [field: SerializeField] public LevelType CurrentLevel { get; private set; }

        public void SetCurrentLevel(LevelType type) => CurrentLevel = type;

        public LevelConfig GetCurrentLevelConfig() => _levels.First(t => t.Type == CurrentLevel);
    }
}