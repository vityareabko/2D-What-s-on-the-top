
public interface ISceneLoad
{
    public void GoToMainMenu();
    public void GoToLevelSelection();
    // public void GoToGameplay(LevelLoadingData levelLoadingData);
    public void GoToGameplay(LevelConfig config);
}
