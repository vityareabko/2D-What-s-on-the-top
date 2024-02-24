
public class SceneLoadMediator : ISceneLoad
{
    private ISimpleSceneLoad _simpleSceneLoad;
    private ILevelLoader _levelLoader;

    public SceneLoadMediator(ISimpleSceneLoad simpleSceneLoad, ILevelLoader levelLoader)
    {
        _levelLoader = levelLoader;
        _simpleSceneLoad = simpleSceneLoad;
    }
    
    public void GoToMainMenu() => _simpleSceneLoad.Load(SceneID.MainMenu);

    public void GoToLevelSelection() => _simpleSceneLoad.Load(SceneID.SelectLevel);

    
    // public void GoToGameplay(LevelLoadingData levelLoadingData) => _levelLoader.Load(levelLoadingData);
    
    public void GoToGameplay(LevelConfig config) => _levelLoader.Load(config);
}
