using Systems.ResourcesLoaderSystem;
using Zenject;

namespace Installers
{
    public class SystemsGlobalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindResourceLoader();
        }

        private void BindResourceLoader() => Container.BindInterfacesAndSelfTo<ResourceLoaderSystem>().AsSingle();
    }
}