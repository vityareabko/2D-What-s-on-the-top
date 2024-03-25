using Extensions;
using UI.MVP;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu.InventoryPanel
{
    public interface IInventoryPanelView : IView
    {
        public Transform ParentResourceItems { get; }
    }
    
    public class InventoryPanelView : BasePanelView, IInventoryPanelView
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Transform _parentResourceItems;
        [SerializeField] private Button _closeButton;
        
        public override PanelType PanelType => PanelType.Inventory;

        public Transform ParentResourceItems => _parentResourceItems;

        protected override void OnAwake()
        {
            base.OnAwake();
            _closeButton.onClick.AddListener(OnClickCloseButton);
        }

        protected override void OnShow()
        {
            base.OnShow();
            _rectTransform.AnimateFromOutsideToPosition(_rectTransform.anchoredPosition, RectTransformExtensions.Direction.Right, 0.1f);
        }

        protected override void OnDestroyInner()
        {
            base.OnDestroyInner();
            _closeButton.onClick.RemoveListener(OnClickCloseButton);
        }

        private void OnClickCloseButton() => Hide();
        
    }
}