namespace UIController.Screen
{
    public interface IScreenPresenter<in T> : IPresenter<T> where T : IScreenView
    {
    }
}