using UI;
using UI.GameScreenDefeatView;
using UnityEngine;
using Zenject;

public class GameplayUIInstaller : MonoInstaller
{
    [SerializeField] private GameScreenView _gameScreenPrefab;
    [SerializeField] private GameScreenDefeatView _defeatScreenPrefab;
    [SerializeField] private Transform _parentGameScreenView;
    
    public override void InstallBindings()
    {
        BindGameplayScreenMVP();
        BindDefeatScreenMVP();
    }

    private void BindGameplayScreenMVP()
    {
        Container.Bind<IGameScreenModel>().To<GameScreenModel>().AsSingle();
        Container.Bind<IGameSreenView>().FromComponentInNewPrefab(_gameScreenPrefab).UnderTransform(_parentGameScreenView).AsSingle();
        Container.BindInterfacesAndSelfTo<GameScreenPresenter>().AsSingle();
    }

    private void BindDefeatScreenMVP()
    {
        Container.Bind<IGameScreenDefeatModel>().To<GameScreenDefeatModel>().AsSingle();
        Container.Bind<IGameScreenDefeatView>().FromComponentInNewPrefab(_defeatScreenPrefab).UnderTransform(_parentGameScreenView).AsSingle();
        Container.BindInterfacesAndSelfTo<GameScreenDefeatPresenter>().AsSingle();
    }
}
