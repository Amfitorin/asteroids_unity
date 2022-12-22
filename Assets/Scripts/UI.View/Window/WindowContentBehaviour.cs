using System;
using UI.ViewApi.View;
using UIModel.Window;
using UnityEngine;

namespace UI.View.Window
{
    public class WindowContentBehaviour : MonoBehaviour, IWindowView
    {
        [SerializeField]
        private string _headerKey;


        public Action CloseWindowAction;


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