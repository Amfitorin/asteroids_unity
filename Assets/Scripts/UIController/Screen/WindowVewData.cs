using UnityEngine;

namespace UIController.Screen
{
    public class WindowViewData<T> : WindowData where T : IScreenView
    {
        public GameObject ContentPrefab;
        public IScreenPresenter<T> Presenter;
    }
}