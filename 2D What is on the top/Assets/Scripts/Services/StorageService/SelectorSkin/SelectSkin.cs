using System;
using Services.StorageService;
using Services.StorageService.JsonDatas;
using UI.MainMenu.ShopSkinsScreen;
using UnityEngine;

namespace MyNamespace.Services.StorageService.SelectorSkin
{

    public interface ISelectSkin 
    {
        public ShopSkinType CurrentHeroSkin { get; }
        public ShopSkinType CurrentShieldSkin { get; }
        public void Select(ShopSkinType type, ShopSkinTabType tabType);
    }

    public class SelectSkin : ISelectSkin
    {
        public ShopSkinType CurrentHeroSkin { get; private set; }
        public ShopSkinType CurrentShieldSkin { get; private set; }

        private IStorageService _storageService;
        private PlayerJsonData _data;
        
        public SelectSkin(IStorageService storageService)
        {
            _storageService = storageService;
            LoadData();
            CurrentHeroSkin = _data.SelectedHeroSkin;
            CurrentShieldSkin = _data.SelectedShieldSkin;
        }
        
        public void Select(ShopSkinType type, ShopSkinTabType tabType)
        {
            LoadData(); // ВАЖНО (это не трогаем) - это мы загружаем данные для того чтобы получить обновленый саписок разблокированых скинов
            
            
            if (TryToSelectSkin(type) == false)
                throw new ArgumentException($"skin - {type} is block");

            switch (tabType)
            {
                case ShopSkinTabType.HeroTab:
                    CurrentHeroSkin = type;
                    _data.SelectedHeroSkin = type;
                    break;
                case ShopSkinTabType.ShieldTab:
                    CurrentShieldSkin = type;
                    _data.SelectedShieldSkin = type;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tabType), tabType, null);
            }
            
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
                    Debug.Log($"update CurrentHeroSkin: {CurrentHeroSkin} success");
                else
                    Debug.Log($"Failed save - CurrentHeroSkin: {CurrentHeroSkin}");
            });
        }
    }
}
    