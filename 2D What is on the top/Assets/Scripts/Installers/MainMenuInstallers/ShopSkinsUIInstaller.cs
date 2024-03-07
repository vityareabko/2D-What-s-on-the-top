using UI.MainMenu.ShopSkinsScreen;
using UnityEngine;
using Zenject;

namespace Installers.MainMenuInstallers
{
    public class ShopSkinsUIInstaller : MonoInstaller
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private ShopSkinsScreenView _shopSkinsScreenView;

        public override void InstallBindings()
        {
            BindShopSkinsMVPInstaller();
        }

        private void BindShopSkinsMVPInstaller()
        {
            Container.Bind<IShopSkinsScreenModel>().To<ShopSkinsScreenModel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ShopSkinsScreenView>().FromComponentInNewPrefab(_shopSkinsScreenView).UnderTransform(_parent).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ShopSkinsScreenPresenter>().AsSingle().NonLazy();
        }
    }
}