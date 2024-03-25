using UnityEngine;
using Zenject;

namespace Installers
{
    public class ConfigsGlobalInstaller : MonoInstaller
    {
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private ShopSkinDB _shopSkinDB;
        [SerializeField] private ShopSkinFactory _shopSkinFactory;
        [SerializeField] private InventoryFactory _inventoryFactory;
        
        public override void InstallBindings()
        {
            Container.Bind<PlayerStats>().FromInstance(playerStats).AsSingle();
            Container.Bind<ShopSkinDB>().FromInstance(_shopSkinDB).AsSingle();
            Container.Bind<ShopSkinFactory>().FromInstance(_shopSkinFactory).AsSingle();
            Container.Bind<InventoryFactory>().FromInstance(_inventoryFactory).AsSingle();
        }
    }
}