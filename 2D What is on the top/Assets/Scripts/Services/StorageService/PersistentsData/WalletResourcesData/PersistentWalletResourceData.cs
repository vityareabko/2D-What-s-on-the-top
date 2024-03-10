using Services.StorageService;
using Services.StorageService.JsonDatas;
using UnityEngine;

namespace PersistentData
{
    public class PersistentWalletResourceData : IPersistentResourceData
    {
        public ResourcesJsonData ResourcesJsonData { get; private set; }

        private IStorageService _storageService;
        
        public PersistentWalletResourceData(IStorageService storageService)
        {
            _storageService = storageService;
            LoadData();
        }

        public void SaveData()
        {
            if (ResourcesJsonData == null)
                LoadData();
            
            _storageService.Save(StorageKeysType.Resources, ResourcesJsonData, (b) =>
            {
                if (b)
                    Debug.Log("Succes save Resource data");
                else
                    Debug.Log("failed save ResorceData");
            });
        }

        private void LoadData()
        {
            _storageService.Load<ResourcesJsonData>(StorageKeysType.Resources, (data) =>
            {
                if (data != null)
                    ResourcesJsonData = data;
                else
                {
                    ResourcesJsonData = new ResourcesJsonData();
                    SaveData();
                }
            });
        }
    }
}