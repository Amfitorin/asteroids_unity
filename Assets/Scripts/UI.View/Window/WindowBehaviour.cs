using Core.Utils.Extensions;
using JetBrains.Annotations;
using TMPro;
using UI.View.Screen;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View.Window
{
    public class WindowBehaviour : WindowBase
    {
        [SerializeField]
        private Transform _container;

        [SerializeField]
        private Image _windowBack;

        public WindowContentBehaviour WindowContentBehaviour { get; set; }

        public Transform Container => _container;

        public Image WindowBack => _windowBack;

        protected void Awake()
        {
            if (_windowBack != null) _windowBack.color = Color.gray;
        }

        [UsedImplicitly]
        public void CloseWindow()
        {
            WindowCanvas.Current.Close();
        }
    }
}