using System;
using Services.StorageService;
using Services.StorageService.JsonDatas;
using UnityEngine;

namespace WalletResources
{
    public interface IWalletResource
    {
        public bool HasEnoughResourceAmount(ResourceTypes type, int amount);
        public bool Spend(ResourceTypes type, int amount);
        public void Add(ResourceTypes type, int amount);
    }

    public class WalletResource : IWalletResource
    {
        public IStorageService _storageService;
        public ResourcesJsonData _storageResourceData;
        
        private WalletResource(IStorageService storageService)
        {
            _storageService = storageService;
            
            _storageService.Load<ResourcesJsonData>(StorageKeysType.Resources, (data) =>
            {
                if (data != null)
                    _storageResourceData = data;
                else
                    _storageResourceData = new ResourcesJsonData();
                
            });
        }
        
        public bool Spend(ResourceTypes type, int amount)
        {
    
            if (HasEnoughResourceAmount(type, amount) == false)
            {
                Debug.Log($"Don't have enough {type} resource");
                return false;
            }
            
            _storageResourceData.Resources[type] -= amount;
            
            _storageService.Save(StorageKeysType.Resources, _storageResourceData, (b) =>
            {
                if (b)
                    Debug.Log($"{type} resource success save, resource amount - {_storageResourceData.Resources[type]}");
                else
                    Debug.Log($"{type} - failed save");
            });
            
            return true;
        }

        public void Add(ResourceTypes type, int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException($"resource {type} to amount to Add < 0");


            if (_storageResourceData.Resources.ContainsKey(type))
                _storageResourceData.Resources[type] += amount;
            else
                _storageResourceData.Resources[type] = amount;
                
            
            _storageService.Save(StorageKeysType.Resources, _storageResourceData, (b) =>
            {
                if (b)
                    Debug.Log($"{type} resource success save, resource amount - {_storageResourceData.Resources[type]}");
                else
                    Debug.Log($"{type} - failed save");
            });
        }

        public bool HasEnoughResourceAmount(ResourceTypes type, int amount)
        {
            if (_storageResourceData.Resources.ContainsKey(type) == false) // сдесь проверяем если вообще есть в сохранения такой рессурс если его нету то он никогда небыл добовлен поэтому он 0
                return false;

            if (_storageResourceData.Resources[type] >= amount)
                return true;

            return false;
        }
    }
}