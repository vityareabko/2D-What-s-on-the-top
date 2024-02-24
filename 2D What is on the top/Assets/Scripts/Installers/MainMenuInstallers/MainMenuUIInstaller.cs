using UI.MainMenu.MainMenuScreen;
using UnityEngine;
using Zenject;

namespace Installers.MainMenuInstallers
{
    public class MainMenuUIInstaller : MonoInstaller
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private MainMenuScreenView _mainMenuScreen;
        public override void InstallBindings()
        {
            BindMainMenuScreenMVP();
        }

        public void BindMainMenuScreenMVP()
        {
            Container.BindInterfacesAndSelfTo<MainMenuScreenModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<MainMenuScreenView>().FromComponentInNewPrefab(_mainMenuScreen).UnderTransform(_parent).AsSingle();
            Container.BindInterfacesAndSelfTo<MainMenuScreenPresenter>().AsSingle();
        }
    }
}