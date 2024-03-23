using System;
using System.Collections.Generic;
using DG.Tweening;
using Extensions;
using UI.MVP;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UpgradeStatsPanel
{
    public interface IUpgradeStatsPanelView : IView<IUpgradeStatsPanelPresenter>
    {
        public void UpdateStateItem(PlayerStats playerStats);
    }

    public class UpgradeStatsPanelView : BasePanelView, IUpgradeStatsPanelView
    {
        public override PanelType PanelType => PanelType.UpgradeStat;
        
        [SerializeField] private RectTransform _statsPanel;
        
        [SerializeField] private Toggle _upgradeStatPanelToggle;

        [SerializeField] private List<UpgradeStatItem> _statItems;
        
        public IUpgradeStatsPanelPresenter Presentor { get; private set; }
        
        public void InitPresentor(IUpgradeStatsPanelPresenter presentor) => Presentor = presentor;
        
        protected override void OnAwake()
        {
            base.OnAwake();

            foreach (var upgradeStatbutton in _statItems)
                upgradeStatbutton.UpgradeButtonClicked += OnUpgradeButtonClicked;
            
            _upgradeStatPanelToggle.onValueChanged.AddListener(OnToggleClickedUpgradePanel);

        }

        protected override void OnShow()
        {
            if (_statsPanel.anchoredPosition.x > 0)
                _statsPanel.anchoredPosition *= -1f;
            
            base.OnShow();

            ShowPanelAnimation();
        }

        public override void Hide(Action callBack = null)
        {
            HidePanelAnimation(() => base.Hide(callBack));
        }


        public void UpdateStateItem(PlayerStats playerStats)
        {
            foreach (var statItem in _statItems)
            {
                var level = (int)playerStats.CurrentPlayerStats[statItem.StatType];

                statItem.UpdateStatData
                (
                    playerStats.GetStatLevelByType(statItem.StatType),
                    playerStats.GetStatPriceAndIconResourceByType(statItem.StatType).Item1,
                    playerStats.GetStatPriceAndIconResourceByType(statItem.StatType).Item2,
                    playerStats.GetStatCurrentAndNextLevelByType(statItem.StatType).Item1,
                    playerStats.GetStatCurrentAndNextLevelByType(statItem.StatType).Item2 ?? null
                );
            }
        }
        
        protected override void OnDestroyInner()
        {
            base.OnDestroyInner();
            
            foreach (var upgradeStatbutton in _statItems)
                upgradeStatbutton.UpgradeButtonClicked -= OnUpgradeButtonClicked;
            
            _upgradeStatPanelToggle.onValueChanged.RemoveListener(OnToggleClickedUpgradePanel);
        }

        private void HidePanelAnimation(Action callback)
        {
            var anchorPositionIsStatPanelActive = -1f;

            if (_statsPanel.anchoredPosition.x > 0)
                anchorPositionIsStatPanelActive = -1;
            else
                anchorPositionIsStatPanelActive = 1f;
            
            var anchorPosition = _statsPanel.anchoredPosition;
            _statsPanel.DOAnchorPosX((_statsPanel.anchoredPosition.x * anchorPositionIsStatPanelActive) - 300f, 0.2f).OnComplete(() =>
            {
                callback?.Invoke();
                _statsPanel.anchoredPosition = anchorPosition;
            });
        }

        private void ShowPanelAnimation()
        {
            var startPositionStatPanel = _statsPanel.anchoredPosition;
            startPositionStatPanel.x -= 300f;
            _statsPanel.anchoredPosition = startPositionStatPanel;
            _statsPanel.DOAnchorPosX(_statsPanel.anchoredPosition.x + 300f, 0.4f);
        }

        private void OnToggleClickedUpgradePanel(bool isOn)
        {
            if (isOn)
                Presentor.UpdateStatItems();
            
            _upgradeStatPanelToggle.interactable = false;
            _statsPanel.DOAnchorPos(_statsPanel.anchoredPosition * -1f, .3f).SetEase(Ease.OutBounce).OnComplete(() => _upgradeStatPanelToggle.interactable = true);
        }
        
        private void OnUpgradeButtonClicked(PlayerStatType statType) => 
            Presentor.OnUpgradeStatsButtonClicked(statType);

    }
}