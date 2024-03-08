using System;
using Services.StorageService;
using Services.StorageService.JsonDatas;
using UnityEngine;

namespace MyNamespace.Services.StorageService.SelectorSkin
{

    public interface ISelectSkin 
    {
        public ShopSkinType CurrentSkin { get; }
        public void Select(ShopSkinType type);
    }

    public class SelectSkin : ISelectSkin
    {
        public ShopSkinType CurrentSkin { get; private set; }

        private IStorageService _storageService;
        private PlayerJsonData _data;
        
        public SelectSkin(IStorageService storageService)
        {
            _storageService = storageService;
            LoadData();
            CurrentSkin = _data.SelectedSkin;
        }
        
        public void Select(ShopSkinType type)
        {
            LoadData(); // ВАЖНО (это не трогаем) - это мы загружаем данные для того чтобы получить обновленый саписок разблокированых скинов
            
            if (TryToSelectSkin(type) == false)
                throw new ArgumentException($"skin - {type} is block");

            CurrentSkin = type;
            _data.SelectedSkin = type;
            
            SaveData();
        }

        private bool TryToSelectSkin(ShopSkinType type)
        {
            if (_data.AvailableSkins.Contains(type) == false)
                return false;

            return true;
        }

        private void LoadData()
        {
            _storageService.Load<PlayerJsonData>(StorageKeysType.PlayerData, (data) =>
            {
                if (data != null)
                    _data = data;
                else
                    _data = new PlayerJsonData();
            });
        }

        private void SaveData()
        {
            _storageService.Save(StorageKeysType.PlayerData, _data, (b) =>
            {
                if (b)
                    Debug.Log($"update CurrentSkin: {CurrentSkin} success");
                else
                    Debug.Log($"Failed save - CurrentSkin: {CurrentSkin}");
            });
        }
    }
}
    