using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEngine;

namespace Gameplay.Player
{
    public interface IPlayerComponent
    {
        event Action OnDied;
        Bounds GetBounds();
        void ApplySpeed(float percent);
    }
}