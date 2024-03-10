using Services.StorageService;
using Services.StorageService.JsonDatas;
using UnityEngine;

namespace PersistentPlayerData
{
    public class PersistentData : IPersistentData
    {
        private PlayerJsonData _playerData;
        private IStorageService _storageService;

        public PersistentData(IStorageService storageService)
        {
            _storageService = storageService;
            LoadPlayerData();
        }

        public PlayerJsonData PlayerData => _playerData;
        
        public void SavePlayerData()
        {
            _storageService.Save(StorageKeysType.PlayerData, _playerData, (success) =>
            {
                if (success)
                    Debug.Log("succes save PlayerData");
                else
                    Debug.Log($"failed save PlayerData");
        
            });
        }
        
        private void LoadPlayerData()
        {
            _storageService.Load<PlayerJsonData>(StorageKeysType.PlayerData, (data) =>
            {
                if (data != null)
                    _playerData = data;
                else
                {
                    _playerData = new PlayerJsonData();
                    _storageService.Save(StorageKeysType.PlayerData, _playerData);
                }
            });
        }
        

    }
}