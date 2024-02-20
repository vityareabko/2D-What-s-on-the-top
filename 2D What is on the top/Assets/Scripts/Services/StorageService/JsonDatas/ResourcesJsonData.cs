using System.Collections.Generic;
using Newtonsoft.Json;
using ResourcesCollector;

namespace Services.StorageService.JsonDatas
{
    public class ResourcesJsonData
    {
        [JsonProperty(PropertyName = "resources")]
        public Dictionary<ResourceStorageTypes, int> Resources = new();

        // public Dictionary<ResourceStorageTypes, int> AdhesivePlaster             = new();
        // public Dictionary<ResourceStorageTypes, int> CrumpledPlasticBootle       = new();
        // public Dictionary<ResourceStorageTypes, int> CrumpledSodaCan             = new();
        // public Dictionary<ResourceStorageTypes, int> Glasses                     = new();
        // public Dictionary<ResourceStorageTypes, int> PieceOfFabricsGreen         = new();
        // public Dictionary<ResourceStorageTypes, int> PieceOfFabricsYellow        = new();
        // public Dictionary<ResourceStorageTypes, int> PieceOfFabricsOrange        = new();
        // public Dictionary<ResourceStorageTypes, int> PlasticlBagYellow           = new();
        // public Dictionary<ResourceStorageTypes, int> PlasticlBagBlue             = new();
        // public Dictionary<ResourceStorageTypes, int> PlasticlBagGreen            = new();
        // public Dictionary<ResourceStorageTypes, int> PlasticlBagRed              = new();
        // public Dictionary<ResourceStorageTypes, int> PlasticlBagPurple           = new();
        // public Dictionary<ResourceStorageTypes, int> PlateSimple                 = new();
        // public Dictionary<ResourceStorageTypes, int> PlateRed                    = new();
        // public Dictionary<ResourceStorageTypes, int> PlateFlowers                = new();
        // public Dictionary<ResourceStorageTypes, int> PlateMixColored             = new();
        // public Dictionary<ResourceStorageTypes, int> Spoon                       = new();
        // public Dictionary<ResourceStorageTypes, int> ToothBrushRed               = new();
        // public Dictionary<ResourceStorageTypes, int> ToothBrushWhithe            = new();

        // ...
        // другие словари с рессурсами 
        // public Dictionary<ResourceStorageTypes, int> Rubins = new();
    }
}