using DG.Tweening;
using UnityEngine;

namespace UIController.Screen
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