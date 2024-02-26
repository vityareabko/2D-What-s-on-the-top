using UnityEngine.SceneManagement;

namespace Systems.SceneSystem
{
    public interface ISceneSystem
    {
        public void ReloadScene();
    }

    public class SceneSystem : ISceneSystem
    {
        public void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}