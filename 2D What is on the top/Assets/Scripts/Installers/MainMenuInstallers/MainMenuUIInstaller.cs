using UI.MainMenu;
using UnityEngine;
using UpgradeStatsPanel;
using Zenject;

namespace Installers.MainMenuInstallers
{
    public class MainMenuUIInstaller : MonoInstaller
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private MainMenuScreenView _mainMenu;
        [SerializeField] private UpgradeStatsPanelView _upgradeStatsPanelView;
        

        public override void InstallBindings()
        {
            BindMainMenuMVP();
        }

        private void BindMainMenuMVP()
        {
            Container.Bind<IMainMenuModel>().To<MainMenuScreenModel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MainMenuScreenView>().FromComponentInNewPrefab(_mainMenu).UnderTransform(_parent).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MainMenuScreenPresenter>().AsSingle().NonLazy();
            
            BindMainMenuUpgradeStatsPanel();
        }

        private void BindMainMenuUpgradeStatsPanel()
        {
            Container.BindInterfacesAndSelfTo<UpgradeStatsPanelView>().FromMethod(context =>
            {
                var mainMenuTransform = context.Container.Resolve<MainMenuScreenView>().transform;
                return Container.InstantiatePrefabForComponent<UpgradeStatsPanelView>(_upgradeStatsPanelView, mainMenuTransform);
            }).AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<UpgradeStatsPanelPresenter>().AsSingle();
        }
    }
}