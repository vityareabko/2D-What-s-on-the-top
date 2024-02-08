using UnityEngine;

namespace UI.MVP
{
    public abstract class BaseView : MonoBehaviour, IView
    {
        public abstract ViewType Type { get; }
        
        [SerializeField] private bool _hideOnAwake;

        private void Awake()
        {
            if (_hideOnAwake) Hide();
            
            OnAwake();
        }

        private void Start()
        {
            OnStart();
        }

        private void OnDestroy()
        {
            OnDestroyInner();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            OnShow();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            OnHide();
        }

        protected virtual void OnAwake(){} // вызывается сразу после Awake
        protected virtual void OnStart(){} // вызывается сразу после Start
        protected virtual void OnShow(){} // вызывается сразу после Show
        protected virtual void OnHide(){} // вызывается сразу после Hide
        protected virtual void OnDestroyInner(){} 
    }
}