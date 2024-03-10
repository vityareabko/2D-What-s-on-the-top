using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Services.StorageService.JsonDatas
{
    public class ResourcesJsonData
    {
        [JsonProperty(PropertyName = "resources")]
        public Dictionary<ResourceTypes, int> Resources = new();
        
        public bool Spend(ResourceTypes type, int amount)
        {
            if (HasEnoughResourceAmount(type, amount) == false)
            {
                Debug.Log($"Don't have enough {type} resource");
                return false;
            }
            
            Resources[type] -= amount;

            return true;
        }

        public void Add(ResourceTypes type, int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException($"resource {type} to amount to Add < 0");


            if (Resources.ContainsKey(type))
                Resources[type] += amount;
            else
                Resources[type] = amount;
        }

        public bool HasEnoughResourceAmount(ResourceTypes type, int amount)
        {
            if (Resources.ContainsKey(type) == false) // сдесь проверяем если вообще есть в сохранения такой рессурс если его нету то он никогда небыл добовлен поэтому он 0
                return false;

            if (Resources[type] >= amount)
                return true;

            return false;
        }
    }
}