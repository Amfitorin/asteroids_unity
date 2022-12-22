using DG.Tweening;
using UIModel.Window;
using UnityEngine;

namespace UI.View.Screen
{
    [DisallowMultipleComponent]
    public class WindowBase : MonoBehaviour
    {
        private Sequence _sequence;

        public virtual void BeforeOpen(WindowData data)
        {
        }

        public virtual void AfterClose(WindowData data)
        {
        }
    }
}