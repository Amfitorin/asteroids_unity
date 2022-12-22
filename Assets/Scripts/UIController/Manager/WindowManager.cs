using System.Collections.Generic;
using System.Linq;
using Core.Singleton;
using CoreMechanics.ObjectLinks.UnityObjectLink;
using UI.View.Screen;
using UI.ViewApi.View;
using UIController.Screen;
using UIModel.Window;
using UnityEngine;

namespace UIController.Manager
{
    public class WindowManager : Singleton<WindowManager>, IWindowManager
    {
        private readonly Stack<WindowData> _stackWindows = new();

        private readonly Dictionary<WindowBehaviour, WindowData> _windows = new();
        private bool _isScreenOpening;

        private bool _isWindowOpening;
        private GameObject _processWindow;

        private WindowData CurrentData { get; set; }
        public ScreenBehaviour CurrentScreen { get; private set; }

        public void OpenScreen<TView>(GameObjectLink prefab, IScreenPresenter<TView> presenter = null,
            bool pushCurrentToStack = true)
            where TView : class, IScreenView
        {
            if (CurrentData?.Prefab == prefab)
            {
                if (prefab?.Resource != null)
                    Debug.LogFormat("[WindowManager] ScreenBehaviour of {0} is already opened.", prefab.Resource.name);
                return;
            }

            OpenScreen(prefab, new WindowViewData<TView> { Presenter = presenter }, pushCurrentToStack);
        }

        public void CloseNotAllScreens()
        {
            if (_stackWindows.Count == 0) return;

            while (_stackWindows.Count > 1) _stackWindows.Pop();

            var data = _stackWindows.Peek();
            _stackWindows.Clear();
            OpenScreen(data.Prefab, data, false);
        }

        public void OpenWindow<TView>(GameObjectLink prefab, IScreenPresenter<TView> presenter)
            where TView : class, IWindowView
        {
            OpenWindow(null, new WindowViewData<TView>
            {
                ContentPrefab = prefab,
                Presenter = presenter
            });
        }

        public GameObjectLink GetCurrentScreen()
        {
            return CurrentData != null ? CurrentData.Prefab : GameObjectLink.Empty;
        }

        public void CloseAllWindows()
        {
            foreach (var window in _windows.ToArray()) window.Key.CloseWindow();
        }

        private void OpenScreen(GameObjectLink prefabLink, WindowData windowData = null,
            bool pushCurrentToStack = true)
        {
            GameObject prefab = prefabLink;
            var screen = prefab.GetComponent<ScreenBehaviour>();
            if (screen == null)
            {
                Debug.LogErrorFormat("[WindowManager] Prefab {0} is not ScreenBehaviour", prefab.name);
                return;
            }

            if (_isScreenOpening)
                Debug.LogErrorFormat("[WindowManager] The screen is already opening {0}", prefab.name);

            var data = windowData ?? new WindowData();
            data.SetupPrefab(prefabLink);
            OpenScreenRoutine(data, pushCurrentToStack);
        }

        private void OpenScreenRoutine(WindowData data, bool pushCurrentToStack)
        {
            _isScreenOpening = true;

            if (CurrentScreen != null)
            {
                if (pushCurrentToStack) _stackWindows.Push(CurrentData);

                CloseScreenRoutine();
            }

            var windowObject = Object.Instantiate(data.Prefab.Resource, WindowCanvas.Current.ScreensRectTransform);

            var screen = windowObject.GetComponent<ScreenBehaviour>();

            screen.BeforeOpen(data);

            CurrentScreen = screen;
            CurrentData = data;

            _isScreenOpening = false;
        }

        private void CloseScreenRoutine()
        {
            var screen = CurrentScreen;
            if (screen == null) return;

            DestroyCurrentScreen();
        }

        private void DestroyCurrentScreen()
        {
            var screen = CurrentScreen;
            if (screen == null) return;

            CloseAllWindows();
            screen.AfterClose(CurrentData);
            Object.Destroy(screen.gameObject);
            CurrentScreen = null;
            CurrentData = null;
        }

        public void CloseScreen()
        {
            if (_stackWindows.Count == 0) return;
            var data = _stackWindows.Pop();
            OpenScreen(data.Prefab, data, false);
        }

        public void CloseAllScreens()
        {
            _stackWindows.Clear();
            DestroyCurrentScreen();
        }

        private void OpenWindow<TView>(GameObjectLink prefab, WindowViewData<TView> windowData)
            where TView : IWindowView
        {
            windowData.SetupPrefab(prefab);

            if (_isWindowOpening)
                if (prefab?.Resource != null)
                    Debug.LogErrorFormat("[WindowManager] The screen or window is already opening {0}",
                        prefab.Resource.name);

            OpenWindowRoutine(windowData);
        }

        private void OpenWindowRoutine<TView>(WindowViewData<TView> windowData) where TView : IWindowView
        {
            _isWindowOpening = true;
            var obj = Object.Instantiate(windowData.Prefab.Resource, WindowCanvas.Current.WindowRectTransform);
            var window = obj.GetComponent<WindowBehaviour>();
            if (window == null)
            {
                Debug.LogErrorFormat("[WindowManager] Prefab {0} is not WindowBehaviour", windowData.Prefab);
                return;
            }

            var objContent = Object.Instantiate(windowData.ContentPrefab, window.Container);
            objContent.transform.SetAsFirstSibling();
            var windowContent = objContent.GetComponent<WindowContentBehaviour>();

            if (windowContent == null)
            {
                Debug.LogError("[WindowManager] The window content is not WindowContentBehaviour");
                return;
            }

            window.WindowContentBehaviour = windowContent;
            windowContent.HeaderSetAction = window.SetHeader;
            windowContent.CloseWindowAction = () => { CloseWindow(window); };
            windowContent.BeforeOpen(windowData);

            _windows.Add(window, windowData);
            _isWindowOpening = false;
        }

        public void CloseWindow(WindowBehaviour window)
        {
            if (window == null)
                return;

            CloseWindowRoutine(window);
        }

        private void CloseWindowRoutine(WindowBehaviour window)
        {
            if (!_windows.TryGetValue(window, out var windowData)) return;

            var windowContent = window.WindowContentBehaviour;
            windowContent.AfterClose(windowData);
            _windows.Remove(window);
            Object.Destroy(window.gameObject);
        }

        protected override void Initialize()
        {
        }
    }
}