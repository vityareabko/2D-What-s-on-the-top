using System;
using System.Collections.Generic;
using Services.StorageService;
using Services.StorageService.JsonDatas;
using UnityEngine;

namespace UnlockerSkins
{
    public interface IUnlockerSkin 
    {
        public List<ShopSkinType> UnlockSkins { get; }
        public void Unlock(ShopSkinType type);
    }

    public class UnlockerSkin : IUnlockerSkin
    {
        private PlayerJsonData _playerData;
        private IStorageService _storageService;

        public List<ShopSkinType> UnlockSkins { get; private set; }
        
        public UnlockerSkin(IStorageService storageService)
        {
            _storageService = storageService;
            LoadPlayerData();
            UnlockSkins = _playerData.AvailableSkins;
            
            InitializeDefaultSkins();
        }

        private void InitializeDefaultSkins()
        {
            if (UnlockSkins.Count < 2)
            {
                _playerData.AvailableSkins.Add(_playerData.SelectedHeroSkin);
                _playerData.AvailableSkins.Add(_playerData.SelectedShieldSkin);
            }

            _storageService.Save(StorageKeysType.PlayerData, _playerData);
        }

        public void Unlock(ShopSkinType type)
        { 
            
            if (HasSkinAlreadyUnlock(type))
                throw new ArgumentException($"{type} - already exist in unlock list");

            _playerData.AvailableSkins.Add(type);
            SaveUnblockSkin(type);
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

        private void SaveUnblockSkin(ShopSkinType type)
        {
            _storageService.Save(StorageKeysType.PlayerData, _playerData, (success) =>
            {
                if (success)
                    Debug.Log($"Skin {type} success unblock and save");
                else
                    Debug.Log($"Skin {type} is unblock but save failed");

            });
        }

        public bool HasSkinAlreadyUnlock(ShopSkinType type)
        {
            if (_playerData.AvailableSkins.Contains(type))
                return true;

            return false;
        }
    }
}