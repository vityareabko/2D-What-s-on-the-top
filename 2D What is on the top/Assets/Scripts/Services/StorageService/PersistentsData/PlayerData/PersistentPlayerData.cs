using Services.StorageService;
using Services.StorageService.JsonDatas;
using UnityEngine;

namespace PersistentData
{
    public class PersistentPlayerData : IPersistentPlayerData
    {
        private PlayerJsonData _playerData;
        private PlayerStats _playerStats;
        private IStorageService _storageService;

        public PersistentPlayerData(IStorageService storageService, PlayerStats playerStats)
        {
            _playerStats = playerStats;
            _storageService = storageService;
            LoadPlayerData();
        }

        public PlayerJsonData PlayerData => _playerData;
        
        public void SaveData()
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
                {
                    _playerData = data;
                    _playerStats.InitializeDataFromLoad(data.GetCurrentStatLevel());
                    
                }
                else
                {
                    _playerData = new PlayerJsonData(_playerStats.CurrentPlayerStats);
                    _storageService.Save(StorageKeysType.PlayerData, _playerData);
                }
            });
        }
        

    }
}