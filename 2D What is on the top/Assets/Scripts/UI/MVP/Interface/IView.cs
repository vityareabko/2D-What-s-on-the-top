using System;

namespace UI.MVP
{
    public interface IView
    {
        public void Show();
        public void Hide(Action callBack = null);
    }
    
    public interface IView<TPresenter> : IView where TPresenter : IPresenter
    {
        public TPresenter Presentor { get; }
        public void InitPresentor(TPresenter presentor);
    }

}