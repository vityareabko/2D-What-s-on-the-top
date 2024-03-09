using System.Collections.Generic;
using Newtonsoft.Json;

namespace Services.StorageService.JsonDatas
{
    public class PlayerJsonData
    {
        [JsonProperty(PropertyName = "open_skins")]
        public List<ShopSkinType> AvailableSkins = new();

        [JsonProperty(PropertyName = "slctd_hero_skin")]
        public ShopSkinType SelectedHeroSkin = ShopSkinType.DwellerArmor;

        [JsonProperty(PropertyName = "slctd_shield_skin")]
        public ShopSkinType SelectedShieldSkin = ShopSkinType.DwellerBucket;


        // можно сохранять статы игрока
    }
}


