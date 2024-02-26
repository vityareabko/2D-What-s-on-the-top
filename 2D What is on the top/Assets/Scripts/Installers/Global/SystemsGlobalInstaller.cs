using Systems.SceneSystem;
using Zenject;

namespace Installers
{
    public class SystemsGlobalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindSceneSystem();
        }
        private void BindSceneSystem() => Container.Bind<ISceneSystem>().To<SceneSystem>().AsSingle();
    }
}