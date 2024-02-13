using ResourcesCollector;
using UnityEngine;
using Zenject;

namespace Installers.GameplayInstallers
{
    public class SystemsGameplayInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindResourcesCollectorSystem();
        }

        private void BindResourcesCollectorSystem()
        {
            Container.Bind<IResourceCollector>().To<ResourceCollector>().AsSingle();
        }
    }
}