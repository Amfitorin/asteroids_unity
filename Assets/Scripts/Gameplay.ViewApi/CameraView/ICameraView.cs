using UnityEngine;

namespace Gameplay.ViewApi.CameraView
{
    public interface ICameraView
    {
        Vector3 ScreenCenter { get; }
        Vector3 RandomScreenPoint { get; }
        Bounds CameraBounds { get; }
    }
}