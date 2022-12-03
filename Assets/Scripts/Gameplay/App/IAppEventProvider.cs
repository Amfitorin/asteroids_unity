using System;

namespace Gameplay.App
{
    public interface IAppEventProvider
    {
        event Action<bool> AppPaused;
        event Action<bool> AppFocused;
        event Action AppQuit;
    }
}