namespace UI.MVP
{
    public interface IView<TPresentor> : IView where TPresentor : IPresentor
    {
        public TPresentor Presentor { get; }
        public void InitPresentor(TPresentor presentor);
    }

    public interface IView
    {
        public void Show();
        public void Hide();
    }
}