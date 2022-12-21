using CoreMechanics.Systems;
using Cysharp.Threading.Tasks;
using Model.Configs.Enemy;
using UnityEngine;

namespace Gameplay.ViewApi.Enemy
{
    public interface INloView : ITokenCancelSource
    {
        UniTask<INloComponent> Spawn(NloConfig config, Vector3 position, Vector3 direction);
        UniTask Destroy();
    }
}