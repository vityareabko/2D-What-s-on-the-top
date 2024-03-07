using UnityEngine;
using Zenject;

namespace Installers
{
    public class ConfigsGlobalInstaller : MonoInstaller
    {
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private ShopSkinDB _shopSkinDB;
        [SerializeField] private ShopSkinFactory _shopSkinFactory;
        
        public override void InstallBindings()
        {
            Container.Bind<PlayerConfig>().FromInstance(_playerConfig).AsSingle();
            Container.Bind<ShopSkinDB>().FromInstance(_shopSkinDB).AsSingle();
            Container.Bind<ShopSkinFactory>().FromInstance(_shopSkinFactory).AsSingle();
        }
    }
}