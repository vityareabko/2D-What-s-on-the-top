using System;
using System.Collections.Generic;
using System.Linq;
using PersistentData;
using UI.MVP;
using UnityEngine;
using VHierarchy.Libs;

namespace UI.MainMenu.InventoryPanel
{
    public interface IInventoryPanelPresentor : IPresenter<IInventoryPanelView>
    {
        public bool IsHide { get; }
    }

    public class InventoryPanelPresentor : IInventoryPanelPresentor
    {
        public IInventoryPanelView View { get; }
        public bool IsHide { get; set; } = false;

        private InventoryFactory _inventoryFactory;
        private IPersistentResourceData _resourceData;
        
        private List<InventoryResourceItem> _inventoryItems = new();
        
        private bool _isInit;

        public InventoryPanelPresentor(IInventoryPanelView view, IPersistentResourceData resourceData, InventoryFactory inventoryFactory)
        {
            View = view;
            _inventoryFactory = inventoryFactory;
            _resourceData = resourceData;
            
            Init();
        }

        public void Show()
        {
            View.Show();
            GenerateResourceItems();
            IsHide = false;
        }

        public void Hide(Action callBack = null)
        {
            View.Hide(callBack);
            IsHide = true;
        }

        public void Init()
        {
            if (_isInit)
                return;
            
            _isInit = true;
            View.InitPresentor(this);
            GenerateResourceItems();
        }

        private void GenerateResourceItems()
        {
            Clear();
            
            var resources = _resourceData.ResourcesJsonData.Resources;

            foreach (var key in resources)
            {
                if (key.Key == ResourceTypes.Coin || key.Key == ResourceTypes.Gem)
                    continue;
                
                var spawnItem = _inventoryFactory.Get(key.Value, key.Key, View.ParentResourceItems);
                _inventoryItems.Add(spawnItem);
            }

            SortInventoryOnAmount();
        }

        private void Clear()
        {
            foreach (var item in _inventoryItems)
                item.gameObject.Destroy();
            
            _inventoryItems.Clear();
        }

        private void SortInventoryOnAmount()
        {
            var sortedItem = _inventoryItems
                .OrderBy(item => item.Category)
                .ThenByDescending(item => item.Amount)
                .ToList();
            
            _inventoryItems.Clear();
            _inventoryItems.AddRange(sortedItem);
            
            for (var i = 0; i < _inventoryItems.Count; i++)
                _inventoryItems[i].transform.SetSiblingIndex(i);
        }

    }
}