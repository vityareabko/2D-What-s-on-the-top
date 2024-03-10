using PersistentPlayerData;
using WalletResources;
using Services.StorageService;
using ShopSkinVisitor.Visitable;
using Zenject;

namespace Installers
{
    public class ServiceInstaller : MonoInstaller
    {
        private IPersistentData _persistentData;
        
        public override void InstallBindings()
        {
            BindStorageService();
            BindWalletResource();
            BindPersistantPlayerData();
            BindShopSkinVisitor();
        }

        private void BindStorageService() => Container.Bind<IStorageService>().To<JsonToFileStorageService>().AsSingle();
        private void BindWalletResource() => Container.Bind<IWalletResource>().To<WalletResource>().AsSingle();
        
        private void BindPersistantPlayerData() => Container.Bind<IPersistentData>().To<PersistentData>().AsSingle();


        private void BindShopSkinVisitor()
        {
            Container.Bind<OpenSkinChecher>().AsSingle();
            Container.Bind<OpenShopSkin>().AsSingle();
            Container.Bind<SelectSkinChecher>().AsSingle();
            Container.Bind<SelectShopSkin>().AsSingle();
            Container.Bind<ClickSkinItemView>().AsSingle();
        }

    }
}