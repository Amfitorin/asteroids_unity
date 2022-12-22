namespace UI.ViewApi.View
{
    public interface IScreenPresenter<in T> : IPresenter<T> where T : IScreenView
    {
    }
}