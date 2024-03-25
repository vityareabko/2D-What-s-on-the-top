using UI.MainMenu;
using UI.MainMenu.InventoryPanel;
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
        [SerializeField] private InventoryPanelView _inventoryPanelView;
        

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
            BindInventoryPanel();
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

        private void BindInventoryPanel()
        {
            Container.BindInterfacesAndSelfTo<InventoryPanelView>().FromMethod(context =>
            {
                var mainMenuTransform = context.Container.Resolve<MainMenuScreenView>().transform;
                return Container.InstantiatePrefabForComponent<InventoryPanelView>(_inventoryPanelView,
                    mainMenuTransform);
            }).AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<InventoryPanelPresentor>().AsSingle();
        }
    }
}