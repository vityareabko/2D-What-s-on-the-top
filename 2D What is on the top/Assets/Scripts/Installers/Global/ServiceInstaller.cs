using Services.StorageService;
using Zenject;

namespace Installers
{
    public class ServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindStorageService();
        }

        private void BindStorageService()
        {
            Container.BindInterfacesAndSelfTo<JsonToFileStorageService>().FromNew().AsSingle();
        }
    }
}