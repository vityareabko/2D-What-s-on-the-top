
public class SceneLoadMediator : ISceneLoad
{
    private ISimpleSceneLoad _simpleSceneLoad;
    private SceneLoader _levelLoader;

    public SceneLoadMediator(ISimpleSceneLoad simpleSceneLoad, SceneLoader levelLoader)
    {
        _levelLoader = levelLoader;
        _simpleSceneLoad = simpleSceneLoad;
    }
    
    public void RestatcCurrentLevel() => _levelLoader.RestartCurrentScene();
    
    public void GoToMainMenu() => _simpleSceneLoad.Load(SceneID.MainMenu);

    public void GoToLevelSelection() => _simpleSceneLoad.Load(SceneID.SelectLevel);
    
    
    public void GoToGameplay(LevelConfig config) => _levelLoader.Load(config);

}
