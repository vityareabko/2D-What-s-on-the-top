using UI;
using UI.GameScreenPause;
using UnityEngine;
using Zenject;

public class GameplayUIInstaller : MonoInstaller
{
    [SerializeField] private GameScreenView _gameScreenPrefab;
    [SerializeField] private GameScreenDefeatView _defeatScreenPrefab;
    [SerializeField] private GameScreenPauseView _pauseScreenPrefab;
    [SerializeField] private Transform _parentGameScreenView;
    
    public override void InstallBindings()
    {
        BindGameplayScreenMVP();
        BindDefeatScreenMVP();
        BindPauseScreenMVP();
    }

    private void BindGameplayScreenMVP()
    {
        Container.Bind<IGameScreenModel>().To<GameScreenModel>().AsSingle();
        Container.Bind<IGameSreenView>().FromComponentInNewPrefab(_gameScreenPrefab).UnderTransform(_parentGameScreenView).AsSingle();
        Container.BindInterfacesAndSelfTo<GameScreenPresenter>().AsSingle();
    }

    private void BindDefeatScreenMVP()
    {
        Container.Bind<IGameScreenDefeatView>().FromComponentInNewPrefab(_defeatScreenPrefab).UnderTransform(_parentGameScreenView).AsSingle();
        Container.BindInterfacesAndSelfTo<GameScreenDefeatPresenter>().AsSingle();
    }

    private void BindPauseScreenMVP()
    {
        Container.Bind<IGameScreenPauseVIew>().FromComponentInNewPrefab(_pauseScreenPrefab).UnderTransform(_parentGameScreenView).AsSingle();
        Container.BindInterfacesAndSelfTo<GameScreenPausePresenter>().AsSingle();
    }
}
