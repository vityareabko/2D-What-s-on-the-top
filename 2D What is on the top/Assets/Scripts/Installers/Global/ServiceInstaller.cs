using MyNamespace.Services.StorageService.SelectorSkin;
using UnlockerSkins;
using WalletResources;
using Services.StorageService;
using Zenject;

namespace Installers
{
    public class ServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindStorageService();
            BindWalletResource();
            BindUnlokerSkin();
            BindSelectSkin();
            
        }

        private void BindSelectSkin() => Container.BindInterfacesAndSelfTo<SelectSkin>().AsSingle();
        
        private void BindUnlokerSkin() => Container.BindInterfacesAndSelfTo<UnlockerSkin>().AsSingle();

        private void BindStorageService() => Container.Bind<IStorageService>().To<JsonToFileStorageService>().AsSingle();
        
        private void BindWalletResource() => Container.Bind<IWalletResource>().To<WalletResource>().AsSingle();


    }
}