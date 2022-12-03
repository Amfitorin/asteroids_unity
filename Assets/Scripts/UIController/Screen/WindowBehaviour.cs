using Core.Utils.Extensions;
using CoreMechanics.Sound;
using JetBrains.Annotations;
using TMPro;
using UIController.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace UIController.Screen
{
    public class WindowBehaviour : WindowBase
    {
        [SerializeField]
        private Transform _container;

        [SerializeField]
        private GameObject _header;

        [SerializeField]
        private TextMeshProUGUI _headerText;

        [SerializeField]
        private Image _windowBack;

        public WindowContentBehaviour WindowContentBehaviour { get; set; }

        public Transform Container => _container;

        public Image WindowBack => _windowBack;

        protected void Awake()
        {
            if (_windowBack != null)
            {
                _windowBack.color = Color.gray;
            }
        }

        public void SetHeader(string header)
        {
            if (header.IsNullOrEmpty())
            {
                _header.SetActive(false);
                return;
            }

            _headerText.text = header;
        }

        [UsedImplicitly]
        public void CloseWindow()
        {
            WindowManager.Instance.CloseWindow(this);
        }
    }
}