using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace CoreMechanics.Scene
{
    public class SceneController : ISceneController
    {
        public void LoadScene(string sceneName, LoadSceneMode sceneMode)
        {
            SceneManager.LoadScene(sceneName, sceneMode);
        }

        public async UniTask LoadSceneAsync(string sceneName, LoadSceneMode sceneMode)
        {
            await SceneManager.LoadSceneAsync(sceneName, sceneMode);
        }
    }
}