using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using VHierarchy.Libs;

namespace Services.StorageService.JsonDatas
{
    public class PlayerJsonData
    {
        // [JsonProperty(PropertyName = "open_skins")]
        // public List<ShopSkinType> AvailableSkins = new();
        //
        //
        // [JsonProperty(PropertyName = "slctd_hero_skin")]
        // public ShopSkinType SelectedHeroSkin;// = ShopSkinType.DwellerArmor;
        //
        // [JsonProperty(PropertyName = "slctd_shield_skin")]
        // public ShopSkinType SelectedShieldSkin;// = ShopSkinType.DwellerBucket;
        
        [JsonProperty(PropertyName = "open_hero_skins")] private List<HeroSkinType> _availableHeroSkins = new();
        [JsonProperty(PropertyName = "open_shield_skins")] private List<ShieldSkinType> _availableShieldSkins = new();

        [JsonProperty(PropertyName = "selected_hero_skin")] private HeroSkinType _selectedHeroSkin;
        [JsonProperty(PropertyName = "selected_shield_skin")] private ShieldSkinType _selectedShieldSkin;

        
        
        public PlayerJsonData(
            HeroSkinType selectedHeroSkin = HeroSkinType.GreenWarrior, 
            ShieldSkinType selectedShieldSkin = ShieldSkinType.DwellerBucket)
        {
            _selectedHeroSkin = selectedHeroSkin;
            _selectedShieldSkin = selectedShieldSkin;
            _availableHeroSkins.Add(_selectedHeroSkin);
            _availableShieldSkins.Add(_selectedShieldSkin);
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

        [JsonIgnore] public IEnumerable<HeroSkinType> AvailableHeroSkins => _availableHeroSkins;
        
        [JsonIgnore] public IEnumerable<ShieldSkinType> AvailableShieldSkins => _availableShieldSkins;

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
        
    }
}


