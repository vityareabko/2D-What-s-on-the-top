using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Installers
{
    public class ConfigsGlobalInstaller : MonoInstaller
    {
        [FormerlySerializedAs("playerBaseData")] [FormerlySerializedAs("_playerConfig")] [SerializeField] private PlayerStats playerStats;
        [SerializeField] private ShopSkinDB _shopSkinDB;
        [SerializeField] private ShopSkinFactory _shopSkinFactory;
        
        public override void InstallBindings()
        {
            Container.Bind<PlayerStats>().FromInstance(playerStats).AsSingle();
            Container.Bind<ShopSkinDB>().FromInstance(_shopSkinDB).AsSingle();
            Container.Bind<ShopSkinFactory>().FromInstance(_shopSkinFactory).AsSingle();
        }
    }
}