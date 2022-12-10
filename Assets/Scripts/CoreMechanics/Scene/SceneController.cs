using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace CoreMechanics.Scene
{
    public class SceneController : ISceneController
    {
        private readonly string _mainScene;
        private readonly string _screenScene;

        public SceneController(string mainScene, string screenScene)
        {
            _mainScene = mainScene;
            _screenScene = screenScene;
        }

        public void LoadScene(string sceneName, LoadSceneMode sceneMode)
        {
            SceneManager.LoadScene(sceneName, sceneMode);
        }

        public async UniTask LoadSceneAsync(string sceneName, LoadSceneMode sceneMode)
        {
            await SceneManager.LoadSceneAsync(sceneName, sceneMode);
        }

        public async UniTask LoadMainSceneAsync()
        {
            await SceneManager.LoadSceneAsync(_mainScene, LoadSceneMode.Additive);
        }

        public async UniTask LoadScreenSceneAsync()
        {
            await SceneManager.LoadSceneAsync(_screenScene, LoadSceneMode.Additive);
        }
    }
}