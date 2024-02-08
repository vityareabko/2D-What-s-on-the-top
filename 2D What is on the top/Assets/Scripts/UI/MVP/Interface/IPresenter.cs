namespace UI.MVP
{
    public interface IPresenter : IView
    {
        public void Init();
    }
    
    public interface IPresenter<out TView> : IPresenter where TView : IView
    {
        public TView View { get; }
    }
    
    public interface IPresenter<out TModel, out TView> : IPresenter where TModel : IModel where TView : IView
    {
        public TModel Model { get; }
        public TView View { get; }
    }
    
}