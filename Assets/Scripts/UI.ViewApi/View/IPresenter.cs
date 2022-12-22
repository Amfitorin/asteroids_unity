namespace UI.ViewApi.View
{
    public interface IPresenter<in T>
    {
        void OnOpen(T view);

        void OnClose(T view);
    }
}