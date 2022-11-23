using System;
using CoreMechanics.App;
using UnityEngine;

namespace Gameplay.App
{
    public class AppEventProvider : MonoBehaviour, IAppEventProvider
    {
        public event Action<bool> AppPaused;
        public event Action<bool> AppFocused;
        public event Action AppQuit;

        private void OnApplicationPause(bool pauseStatus)
        {
            AppPaused?.Invoke(pauseStatus);
        }

        private void OnApplicationQuit()
        {
            AppQuit?.Invoke();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            AppFocused?.Invoke(hasFocus);
        }
    }
}