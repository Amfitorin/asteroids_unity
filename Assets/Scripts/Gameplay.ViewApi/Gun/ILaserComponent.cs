
using UnityEngine;

namespace Gameplay.ViewApi.Gun
{
    public interface ILaserComponent : IExtraGunComponent
    {
        void UpdatePositions(Vector3 first, Vector3 second);
    }
}