using System;
using PersistentData;
using UI.MVP;
using UnityEngine;

namespace UpgradeStatsPanel
{
    public interface IUpgradeStatsPanelPresenter : IPresenter<IUpgradeStatsPanelView>
    {
        public void OnUpgradeStatsButtonClicked(PlayerStatType type);
        public void UpdateStatItems();
    }

    public class UpgradeStatsPanelPresenter : IUpgradeStatsPanelPresenter
    {
        public IUpgradeStatsPanelView View { get; }

        private IPersistentResourceData _persistentResourceData;
        private IPersistentPlayerData _persistentPlayerData;
        private PlayerStats _playerStats;
        
        private bool _isInit;

        public UpgradeStatsPanelPresenter(
            IUpgradeStatsPanelView view, 
            PlayerStats playerStats,
            IPersistentResourceData persistentResourceData,
            IPersistentPlayerData persistentPlayerData
            )
        {
            View = view;

            _persistentResourceData = persistentResourceData;
            _persistentPlayerData = persistentPlayerData;
            
            _playerStats = playerStats;
            Init();
        }

        public void Show()
        {
            View.Show();
            View.UpdateStateItem(_playerStats);
        }

        public void Hide(Action callBack = null) => View.Hide(callBack);

        public void Init()
        {
            if (_isInit)
                return;
            
            View.InitPresentor(this);
            View.UpdateStateItem(_playerStats);
            
            _isInit = true;
        }

        public void UpdateStatItems() => View.UpdateStateItem(_playerStats);

        public void OnUpgradeStatsButtonClicked(PlayerStatType statType)
        {
            if (_playerStats.CanUpgradeToNextLevel(statType))
            {
                var resourceType = _playerStats.GetStatByType(statType).ResourceTypes;
                var amount = _playerStats.GetStatByType(statType).priceAmount;
                
                if (_persistentResourceData.ResourcesJsonData.HasEnoughResourceAmount(resourceType, amount))
                {
                    _persistentResourceData.ResourcesJsonData.Spend(resourceType, amount);
                    
                    _persistentPlayerData.PlayerData.SetCurrentStatLevel(_playerStats.CurrentPlayerStats);
                    _playerStats.UpgradeStatLevel(statType);
                    
                    View.UpdateStateItem(_playerStats);
                    
                    _persistentResourceData.SaveData();
                    _persistentPlayerData.SaveData();
                    
                    Debug.Log("Уровень успешно куплен и обновлен и сохранен");
                }
                else
                {
                    Debug.Log("Не достаточно рессурсов чтобы улучшить уровень");
                }
            }
        }


    }
}