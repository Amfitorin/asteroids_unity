using System;
using UnityEngine;

namespace UI.View.Screen
{
    public class WindowCanvas : MonoBehaviour
    {
        private static WindowCanvas _current;

        [SerializeField]
        private RectTransform _screensRectTransform;

        [SerializeField]
        private RectTransform _windowRectTransform;

        [SerializeField]
        private Canvas _canvas;

        [SerializeField]
        private Camera _uiCamera;

        public event Action CloseScreen;
        public event Action CloseAllScreens;

        public void Close()
        {
            CloseScreen?.Invoke();
        }

        public void CloseAll()
        {
            CloseAllScreens?.Invoke();
        }

        public static WindowCanvas Current
        {
            get
            {
                if (_current == null)
                    _current = FindObjectOfType<WindowCanvas>();

                return _current;
            }
        }

        public RectTransform ScreensRectTransform => _screensRectTransform;

        public RectTransform WindowRectTransform => _windowRectTransform;

        public Camera UICamera => _uiCamera;

        public Canvas Canvas => _canvas;
    }
}