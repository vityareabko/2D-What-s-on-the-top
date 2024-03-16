using Systems.SceneSystem;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GlobalInstaller : MonoInstaller
    {
        [SerializeField] public LoadingGameScreen _loadingGameScreen;
        public override void InstallBindings()
        {
            BindBehavior();
            BindLoadingGameScreen();
            BindScenceManager();
        }
        
        private void BindBehavior() => Container.Bind<CoroutineRunner>().FromNewComponentOnNewGameObject().AsSingle();
        
        private void BindLoadingGameScreen() => Container.Bind<LoadingGameScreen>().FromComponentInNewPrefab(_loadingGameScreen).AsSingle().NonLazy();
        
        private void BindScenceManager() => Container.Bind<SceneController>().AsSingle();
    }
}