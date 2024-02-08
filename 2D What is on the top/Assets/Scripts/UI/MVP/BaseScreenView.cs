namespace UI.MVP
{
    public abstract class BaseScreenView : BaseView
    {
        public override ViewType Type => ViewType.Screens;
        public abstract ScreenType ScreenType { get; }
    }
}