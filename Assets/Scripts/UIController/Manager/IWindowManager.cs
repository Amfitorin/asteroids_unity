using CoreMechanics.ObjectLinks.UnityObjectLink;
using UI.ViewApi.View;

namespace UIController.Manager
{
    public interface IWindowManager
    {
        void OpenScreen<TView>(GameObjectLink prefab,
            IScreenPresenter<TView> presenter = null,
            bool pushCurrentToStack = true)
            where TView : class, IScreenView;

        void OpenWindow<TView>(GameObjectLink prefab, IScreenPresenter<TView> presenter)
            where TView : class, IWindowView;

        void CloseNotAllScreens();
        void CloseAllWindows();
        GameObjectLink GetCurrentScreen();
    }
}