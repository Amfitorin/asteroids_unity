using UI.ViewApi.View;
using UnityEngine;

namespace UIModel.Window
{
    public class WindowViewData<T> : WindowData where T : IScreenView
    {
        public GameObject ContentPrefab;
        public IScreenPresenter<T> Presenter;
    }
}