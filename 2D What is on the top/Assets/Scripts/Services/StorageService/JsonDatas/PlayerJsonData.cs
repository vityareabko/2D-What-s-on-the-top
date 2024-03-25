using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Services.StorageService.JsonDatas
{
    public class PlayerJsonData
    {
        [JsonProperty(PropertyName = "open_hero_skins")] private List<HeroSkinType> _availableHeroSkins = new();
        [JsonProperty(PropertyName = "open_shield_skins")] private List<ShieldSkinType> _availableShieldSkins = new();
        [JsonProperty(PropertyName = "open_levels")] private List<LevelType> _availableLevels = new();
        
        [JsonProperty(PropertyName = "selected_hero_skin")] private HeroSkinType _selectedHeroSkin;
        [JsonProperty(PropertyName = "selected_shield_skin")] private ShieldSkinType _selectedShieldSkin;
        [JsonProperty(PropertyName = "current_level")] private LevelType _currentLevelSelected;

        [JsonProperty(PropertyName = "player_stat")] private Dictionary<PlayerStatType, LevelStatType> _currentPlayerStats;
        
        public PlayerJsonData(
            Dictionary<PlayerStatType, LevelStatType> currentPlayerStats,
            HeroSkinType selectedHeroSkin = HeroSkinType.GreenWarrior, 
            ShieldSkinType selectedShieldSkin = ShieldSkinType.DwellerBucket, 
            LevelType currentLevel = LevelType.Level1
            )
        {
            _currentPlayerStats = currentPlayerStats;
            
            _selectedHeroSkin = selectedHeroSkin;
            _selectedShieldSkin = selectedShieldSkin;
            _currentLevelSelected = currentLevel;
            
            _availableHeroSkins.Add(_selectedHeroSkin);
            _availableShieldSkins.Add(_selectedShieldSkin);
            _availableLevels.Add(_currentLevelSelected);
        }

        [JsonIgnore] public HeroSkinType SelectedHeroSkin
        {
            get => _selectedHeroSkin;
            set
            {
                if (_availableHeroSkins.Contains(value) == false)
                    throw new ArgumentException(nameof(value));
                
                _selectedHeroSkin = value;
            }
        }
        
        [JsonIgnore] public ShieldSkinType SelectedShieldSkin
        {
            get => _selectedShieldSkin;
            set
            {
                if (_availableShieldSkins.Contains(value) == false)
                    throw new ArgumentException(nameof(value));
                
                _selectedShieldSkin = value;
            }
        }

        [JsonIgnore] public LevelType CurrentLevel => _currentLevelSelected;
        
        [JsonIgnore] public IEnumerable<HeroSkinType> AvailableHeroSkins => _availableHeroSkins;
        
        [JsonIgnore] public IEnumerable<ShieldSkinType> AvailableShieldSkins => _availableShieldSkins;

        [JsonIgnore] public IEnumerable<LevelType> AvailableLevels => _availableLevels;

        public void OpenHeroSkin(HeroSkinType type)
        {
            if (_availableHeroSkins.Contains(type))
                throw new ArgumentException(nameof(type));

            _availableHeroSkins.Add(type);
        }

        public void OpenShieldSkin(ShieldSkinType type)
        {
            if (_availableShieldSkins.Contains(type))
                throw new ArgumentException(nameof(type));

            _availableShieldSkins.Add(type);
        }

        public void OpenLevel(LevelType type)
        {
            if (_availableLevels.Contains(type) == false)
                _availableLevels.Add(type);

            _currentLevelSelected = type;
        }

        public void SetCurrentLevel(LevelType type) => _currentLevelSelected = type;

        public void SetCurrentStatLevel(Dictionary<PlayerStatType, LevelStatType> currentStats) =>
            _currentPlayerStats = currentStats;
        
        public Dictionary<PlayerStatType, LevelStatType> GetCurrentStatLevel() => _currentPlayerStats;

    }
}


