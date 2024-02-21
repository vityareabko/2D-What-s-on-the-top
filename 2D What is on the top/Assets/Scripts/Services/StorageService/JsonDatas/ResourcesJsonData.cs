using System.Collections.Generic;
using Newtonsoft.Json;
using ResourcesCollector;

namespace Services.StorageService.JsonDatas
{
    public class ResourcesJsonData
    {
        [JsonProperty(PropertyName = "resources")]
        public Dictionary<ResourceTypes, int> Resources = new();

        // public Dictionary<ResourceTypes, int> AdhesivePlaster             = new();
        // public Dictionary<ResourceTypes, int> CrumpledPlasticBootle       = new();
        // public Dictionary<ResourceTypes, int> CrumpledSodaCan             = new();
        // public Dictionary<ResourceTypes, int> Glasses                     = new();
        // public Dictionary<ResourceTypes, int> PieceOfFabricsGreen         = new();
        // public Dictionary<ResourceTypes, int> PieceOfFabricsYellow        = new();
        // public Dictionary<ResourceTypes, int> PieceOfFabricsOrange        = new();
        // public Dictionary<ResourceTypes, int> PlasticlBagYellow           = new();
        // public Dictionary<ResourceTypes, int> PlasticlBagBlue             = new();
        // public Dictionary<ResourceTypes, int> PlasticlBagGreen            = new();
        // public Dictionary<ResourceTypes, int> PlasticlBagRed              = new();
        // public Dictionary<ResourceTypes, int> PlasticlBagPurple           = new();
        // public Dictionary<ResourceTypes, int> PlateSimple                 = new();
        // public Dictionary<ResourceTypes, int> PlateRed                    = new();
        // public Dictionary<ResourceTypes, int> PlateFlowers                = new();
        // public Dictionary<ResourceTypes, int> PlateMixColored             = new();
        // public Dictionary<ResourceTypes, int> Spoon                       = new();
        // public Dictionary<ResourceTypes, int> ToothBrushRed               = new();
        // public Dictionary<ResourceTypes, int> ToothBrushWhithe            = new();

        // ...
        // другие словари с рессурсами 
        // public Dictionary<ResourceTypes, int> Rubins = new();
    }
}