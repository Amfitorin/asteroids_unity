using System;
using UnityEngine;

namespace UIController.Screen
{
    public class WindowContentBehaviour : MonoBehaviour, IWindowView
    {
        [SerializeField]
        private string _headerKey;


        public Action CloseWindowAction;
        public Action<string> HeaderSetAction;

        public virtual void SetHeader(string header = null)
        {
            HeaderSetAction?.Invoke(header ?? _headerKey);
        }

        public virtual void CloseWindow()
        {
            CloseWindowAction?.Invoke();
        }

        public virtual void BeforeOpen(WindowData windowData)
        {
        }

        public virtual void AfterClose(WindowData windowData)
        {
        }
    }
}