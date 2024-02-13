using System.Collections.Generic;
using Newtonsoft.Json;
using ResourcesCollector;

namespace Services.StorageService.JsonDatas
{
    public class ResourcesJsonData
    {
        [JsonProperty(PropertyName = "coins")]
        public Dictionary<ResourceTypes, int> Coins = new();
        
        // ...
        // другие словари с рессурсами 
        // public Dictionary<ResourceTypes, int> Rubins = new();
    }
}