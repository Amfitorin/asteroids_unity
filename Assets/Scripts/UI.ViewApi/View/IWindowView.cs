namespace UI.ViewApi.View
{
    public interface IWindowView : IScreenView
    {
        void SetHeader(string header = null);
        void CloseWindow();
    }
}