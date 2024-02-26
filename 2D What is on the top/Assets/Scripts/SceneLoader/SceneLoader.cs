using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : ISimpleSceneLoad, ILevelLoader
{
    private ZenjectSceneLoaderWrapper _zenjectSceneLoader;
    private LevelConfig _currentLevelConfig;

    public SceneLoader(ZenjectSceneLoaderWrapper zenjectSceneLoader) => _zenjectSceneLoader = zenjectSceneLoader;

    public void Load(SceneID sceneID)
    {
        if (sceneID == SceneID.GameplayLevel)
            throw new ArgumentException($"{SceneID.GameplayLevel} cannot to be start without Configuration ");
        
        _zenjectSceneLoader.Load(null, (int) sceneID);
    }

    public void Load(LevelConfig config)
    {
        _currentLevelConfig = config;
        _zenjectSceneLoader.Load(container =>
        {
            container.BindInstance(config);
        }, (int) SceneID.GameplayLevel);
    }
    
    public void RestartCurrentScene()
    {
        if (_currentLevelConfig == null)
        {
            Debug.LogError("Cannot restart the scene because the level config is null.");
            return;
        }

        _zenjectSceneLoader.Load(container =>
        {
            // Перепривязываем тот же конфиг уровня, что и был до перезагрузки
            container.BindInstance(_currentLevelConfig);
        }, SceneManager.GetActiveScene().buildIndex);
    }
    
}
