using UI.MainMenu;
using UnityEngine;
using Zenject;

namespace Installers.MainMenuInstallers
{
    public class MainMenuUIInstaller : MonoInstaller
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private MainMenuScreenView _mainMenu;
        
        public override void InstallBindings()
        {
            BindMainMenuMVP();
        }

        private void BindMainMenuMVP()
        {
            Container.Bind<IMainMenuModel>().To<MainMenuScreenModel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MainMenuScreenView>().FromComponentInNewPrefab(_mainMenu).UnderTransform(_parent).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MainMenuScreenPresenter>().AsSingle().NonLazy();
        }
    }
}