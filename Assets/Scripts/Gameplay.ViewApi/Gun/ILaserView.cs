using UnityEngine;

namespace Gameplay.ViewApi.Gun
{
    public interface ILaserView
    {
        void Init(ILaserComponent component);
        void SetPoints(Vector3 first, Vector3 second);
        void SetDirection(Vector3 first, Vector3 direction);
        void RunLaser();
        void DieLaser();
    }
}