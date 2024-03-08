using System.Collections.Generic;
using Newtonsoft.Json;

namespace Services.StorageService.JsonDatas
{
    public class PlayerJsonData
    {
        [JsonProperty(PropertyName = "open_skins")]
        public List<ShopSkinType> AvailableSkins = new();

        [JsonProperty(PropertyName = "slctd_skin")]
        public ShopSkinType SelectedSkin = ShopSkinType.DwellerArmor;
        
        
        // можно сохранять статы игрока
    }
}


