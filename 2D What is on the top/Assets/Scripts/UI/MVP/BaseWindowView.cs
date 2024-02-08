namespace UI.MVP
{
    public abstract class BaseWindowView : BaseView
    {
        public override ViewType Type => ViewType.Windows;
        public abstract WindowType WindowType { get; }
    }
}