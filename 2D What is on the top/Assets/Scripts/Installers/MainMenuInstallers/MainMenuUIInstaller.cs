using UI.MainMenu;
using UnityEngine;
using Zenject;

namespace Installers.MainMenuInstallers
{
    public class MainMenuUIInstaller : MonoInstaller
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private MainMenuVieww _mainMenu;
        
        public override void InstallBindings()
        {
            BindMainMenuMVP();
        }

        private void BindMainMenuMVP()
        {
            Container.Bind<IMainMenuModel>().To<MainMenuModel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MainMenuVieww>().FromComponentInNewPrefab(_mainMenu).UnderTransform(_parent).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MainMenuPresenter>().AsSingle().NonLazy();
        }
    }
}