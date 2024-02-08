namespace UI.MVP
{
    public abstract class BasePopupView : BaseView
    {
        public override ViewType Type => ViewType.Popups;
        public abstract PopupType PopupType { get; }
    }
}