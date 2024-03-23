using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LevelUI
{
    public class LevelUIItem : MonoBehaviour, IPointerClickHandler
    {
        public event System.Action<LevelType> ClickedItem;

        [SerializeField] private Image _lock;
        [SerializeField] private Image _selected;
        
        [SerializeField] private LevelType _type;

        public LevelType Type => _type;

        public void Lock() => _lock.gameObject.SetActive(true);
        public void Unlock() => _lock.gameObject.SetActive(false);

        public void Select() => _selected.gameObject.SetActive(true); // #делать чтобы был выбран или нет - и посмотреть как поведется этот выбор - проблема что иногда плайер не на тот уровне что в Датабасе
        public void Unselect() => _selected.gameObject.SetActive(false);
        
        public void OnPointerClick(PointerEventData eventData) => ClickedItem?.Invoke(Type);
    }
}