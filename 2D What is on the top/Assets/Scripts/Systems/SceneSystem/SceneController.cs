using System.Collections;
using Helper.SceneType;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Systems.SceneSystem
{
    public class SceneController
    {
        public LoadingGameScreen _loadingGameScreen;
        private CoroutineRunner _coroutine;

        public SceneController(CoroutineRunner coroutineRunner, LoadingGameScreen loadingGameScreen)
        {
            _loadingGameScreen = loadingGameScreen;
            _coroutine = coroutineRunner;
        }

        public void RestartCurrentScent()
        {
            _coroutine.StartCoroutine(StartLoadingScene());
        }

        private IEnumerator StartLoadingScene()
        {
            SceneManager.LoadSceneAsync((int)SceneType.LoadingSceneType);
            _loadingGameScreen.Show();
            
            yield return new WaitForSeconds(_loadingGameScreen.LoadingTime);
            
            var operationGameScene = SceneManager.LoadSceneAsync((int)SceneType.GameScene);
            operationGameScene.allowSceneActivation = false;

            while (operationGameScene.isDone == false)
            {
                yield return new WaitForSeconds(_loadingGameScreen.LoadingTime / 2);
                _loadingGameScreen.HideSmooth(() =>
                {
                    operationGameScene.allowSceneActivation = true;
                });
            }
            
        }

    }
}