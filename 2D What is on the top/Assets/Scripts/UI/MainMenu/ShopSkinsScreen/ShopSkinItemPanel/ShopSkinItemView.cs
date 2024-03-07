using System;
using Assets.HeroEditor.Common.Scripts.Common;
using Extensions;
using TMPro;
using UI.MVP;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace UI.MainMenu.ShopSkinItemPanel
{
    public interface IShopSkinItemView : IView
    {
        public event Action<ShopSkinItemView> ClickedOnView; 
        
        public void Unlock();
        public void Lock();

        public void Select();
        public void Unselect();
    }

    public class ShopSkinItemView : BasePanelView, IShopSkinItemView, IPointerClickHandler
    {
        public override PanelType PanelType { get; } = PanelType.ShopItem;

        public event Action<ShopSkinItemView> ClickedOnView;
        
        [SerializeField] private Image _contentImage;
        [SerializeField] private Image _lockPanel;
        [SerializeField] private TMP_Text _price;
        [SerializeField] private Image _selectedText;

        private SkinItemConfig Item;

        public bool IsLock { get; private set; }

        // public int Price => Item.Price;

        public void Initialize(SkinItemConfig config)
        {
            Item = config;
            _contentImage.sprite = config.ShopIcon;
            // IsLock = config.IsLock;
 
            _price.Show(config.Price); // нужно протестить если работает Show этот метод расширения для TMP_Text
        }
        
        protected override void OnAwake() => _selectedText.SetActive(false);

        public void Unlock()
        {
            IsLock = false;
            _lockPanel.SetActive(false);
            _price.SetActive(false);
        }

        public void Lock()
        {
            IsLock = true;
            _lockPanel.SetActive(true);
            _price.SetActive(true);
        }

        public void Select() => _selectedText.SetActive(true);  
        public void Unselect() => _selectedText.SetActive(false);

        public void OnPointerClick(PointerEventData eventData) => ClickedOnView?.Invoke(this);
    }
}