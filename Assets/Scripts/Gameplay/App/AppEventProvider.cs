using System;
using UnityEngine;

namespace Gameplay.App
{
    public class AppEventProvider : MonoBehaviour, IAppEventProvider
    {
        private void OnApplicationFocus(bool hasFocus)
        {
            AppFocused?.Invoke(hasFocus);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            AppPaused?.Invoke(pauseStatus);
        }

        private void OnApplicationQuit()
        {
            AppQuit?.Invoke();
        }

        public event Action<bool> AppPaused;
        public event Action<bool> AppFocused;
        public event Action AppQuit;
    }
}