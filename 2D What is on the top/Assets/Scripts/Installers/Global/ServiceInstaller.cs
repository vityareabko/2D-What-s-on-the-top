using PersistentData;
using Services.StorageService;
using ShopSkinVisitor.Visitable;
using Zenject;

namespace Installers
{
    public class ServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindStorageService();
            // BindWalletResource();
            BindPersistantPlayerData();
            BindPersistantWalletResourceData();
            BindShopSkinVisitor();
        }

        private void BindStorageService() => Container.Bind<IStorageService>().To<JsonToFileStorageService>().AsSingle();
        // private void BindWalletResource() => Container.Bind<IWalletResource>().To<WalletResource>().AsSingle();
        
        private void BindPersistantPlayerData() => Container.BindInterfacesAndSelfTo<PersistentPlayerData>().AsSingle();
        private void BindPersistantWalletResourceData() => Container.BindInterfacesAndSelfTo<PersistentWalletResourceData>().AsSingle();


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