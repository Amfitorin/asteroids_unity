namespace UIController.Screen
{
    public abstract class ScreenView<TView> : ScreenBehaviour where TView : IScreenView
    {
        protected abstract TView View { get; }

        public override void BeforeOpen(WindowData windowData)
        {
            base.BeforeOpen(windowData);
            if (windowData is WindowViewData<TView> data)
                data.Presenter?.OnOpen(View);
        }

        public override void AfterClose(WindowData windowData)
        {
            if (windowData is WindowViewData<TView> data)
                data.Presenter?.OnClose(View);
            base.AfterClose(windowData);
        }
    }
}