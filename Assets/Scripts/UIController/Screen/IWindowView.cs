namespace UIController.Screen
{
    public interface IWindowView : IScreenView
    {
        void SetHeader(string header = null);
        void CloseWindow();
    }
}