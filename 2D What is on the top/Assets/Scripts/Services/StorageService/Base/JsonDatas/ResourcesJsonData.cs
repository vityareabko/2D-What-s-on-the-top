using System.Collections.Generic;
using Newtonsoft.Json;

namespace Services.StorageService.JsonDatas
{
    public class ResourcesJsonData
    {
        [JsonProperty(PropertyName = "resources")]
        public Dictionary<ResourceTypes, int> Resources = new();
    }
}