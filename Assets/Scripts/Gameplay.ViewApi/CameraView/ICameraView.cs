using Core.Utils.Extensions;
using UnityEngine;

namespace Gameplay.ViewApi.CameraView
{
    public interface ICameraView
    {
        float ConstZ { get; }
        Vector3 ScreenCenter { get; }
        Vector3 RandomScreenPoint { get; }
        Rect ViewRect { get; }
        bool IsObjectVisible(CornerRect bounds);
        Vector3 InversePosition(Vector3 position, CornerRect rect);
    }
}