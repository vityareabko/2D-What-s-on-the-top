using UI.MainMenu;
using UnityEngine;
using Zenject;

namespace Installers.MainMenuInstallers
{
    public class MainMenuUIInstaller : MonoInstaller
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private MainMenuView _mainMenuView;
        
        public override void InstallBindings()
        {
            BindMainMenuMVP();
        }

        private void BindMainMenuMVP()
        {
            Container.Bind<IMainMenuModel>().To<MainMenuModel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MainMenuView>().FromComponentInNewPrefab(_mainMenuView).UnderTransform(_parent).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MainMenuPresenter>().AsSingle().NonLazy();
        }
    }
}