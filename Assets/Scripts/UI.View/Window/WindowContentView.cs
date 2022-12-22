using System;
using UI.ViewApi.View;
using UIModel.Window;

namespace UI.View.Window
{
    public abstract class WindowContentView<TView> : WindowContentBehaviour where TView : IWindowView
    {
        protected abstract TView View { get; }

        public override void BeforeOpen(WindowData windowData)
        {
            base.BeforeOpen(windowData);
            if (windowData is WindowViewData<TView> { Presenter: { } } data)
                data.Presenter.OnOpen(View);
        }

        public override void AfterClose(WindowData windowData)
        {
            if (windowData is WindowViewData<TView> { Presenter: { } } data)
                data.Presenter.OnClose(View);
            Action<WindowData> afterClose = base.AfterClose;
            afterClose(windowData);
        }
    }
}