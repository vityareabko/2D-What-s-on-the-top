using System;
using Extensions;
using MyNamespace.Scriptable.Configs.ShopSkins._1111;
using TMPro;
using UI.MVP;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace UI.MainMenu.ShopSkinItemPanel
{
    public interface IShopSkinItemView : IView
    {
        public event Action<SkinItem> ClickedOnView; 
        
        public void Unlock();
        public void Lock();
        
        public void Select();
        public void Unselect();
    }

    public class ShopSkinItemView : BasePanelView, IShopSkinItemView, IPointerClickHandler
    {
        public override PanelType PanelType { get; } = PanelType.ShopItem;
        
        public event Action<SkinItem> ClickedOnView;
        
        [SerializeField] private Image _contentImage;
        [SerializeField] private Image _lockPanel;
        [SerializeField] private TMP_Text _price;
        [SerializeField] private Image _selectedText;

        [SerializeField] private Color _colorDefault;
        [SerializeField] private Color _colorDosentEnoughMoney;
        
        public SkinItem Item;

        public bool IsLock { get; private set; }
        
        public void Initialize(SkinItem config)
        {
            Item = config;
            _contentImage.sprite = config.ShopIcon;
            
            _price.Show(config.PriceCoin); 
            _selectedText.gameObject.SetActive(false);
        }
        

        public void RedPriceTextColor() => _price.color = _colorDosentEnoughMoney;

        public void DefaultPriceTextColor() => _price.color = _colorDefault;

        public void Unlock()
        {
            IsLock = false;
            _lockPanel.gameObject.SetActive(false);
            _price.gameObject.SetActive(false);
        }

        public void Lock()
        {
            IsLock = true;
            _lockPanel.gameObject.SetActive(true);
            _price.gameObject.SetActive(true);
        }

        public void Select() => _selectedText.gameObject.SetActive(true);  
        public void Unselect() => _selectedText.gameObject.SetActive(false);

        public void OnPointerClick(PointerEventData eventData) => ClickedOnView?.Invoke(Item);
    }
}