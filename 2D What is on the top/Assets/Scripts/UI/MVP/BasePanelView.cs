namespace UI.MVP
{
    public abstract class BasePanelView : BaseView
    {
        public override ViewType Type => ViewType.Panels;
        public abstract PanelType PanelType { get; }
    }
}