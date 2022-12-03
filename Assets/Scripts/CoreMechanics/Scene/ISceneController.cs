using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace CoreMechanics.Scene
{
    public interface ISceneController
    {
        void LoadScene(string sceneName, LoadSceneMode sceneMode);
        UniTask LoadSceneAsync(string sceneName, LoadSceneMode sceneMode);

        UniTask LoadMainSceneAsync();
        UniTask LoadScreenSceneAsync();
    }
}