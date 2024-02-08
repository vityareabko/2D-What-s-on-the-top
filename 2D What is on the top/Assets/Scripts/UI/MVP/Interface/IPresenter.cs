namespace UI.MVP
{
    public interface IPresenter<out TView> : IPresentor where TView : IView
    {
        public TView View { get; }
    }
}