using System;
using UnityEngine;


public class SceneLoader : ISimpleSceneLoad, ILevelLoader
{
    private ZenjectSceneLoaderWrapper _zenjectSceneLoader;

    public SceneLoader(ZenjectSceneLoaderWrapper zenjectSceneLoader) => _zenjectSceneLoader = zenjectSceneLoader;

    public void Load(SceneID sceneID)
    {
        if (sceneID == SceneID.GameplayLevel)
            throw new ArgumentException($"{SceneID.GameplayLevel} cannot to be start without Configuration ");
        
        _zenjectSceneLoader.Load(null, (int) sceneID);
    }

    public void Load(LevelConfig config)
    {
        Debug.Log(config);
        _zenjectSceneLoader.Load(container =>
        {
            container.BindInstance(config);
        }, (int) SceneID.GameplayLevel);
    }
    
    // // public void Load(LevelLoadingData levelLoadingData)
    // {
    //     _zenjectSceneLoader.Load(container =>
    //     {
    //         container.BindInstance(levelLoadingData);
    //     }, (int) SceneID.GameplayLevel);
    // }
}
