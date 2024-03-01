using UnityEngine;
using Zenject;

namespace Installers
{
    public class ConfigsGlobalInstaller : MonoInstaller
    {
        [SerializeField] private PlayerConfig playerConfig;
       
        
        public override void InstallBindings()
        {
            Container.Bind<PlayerConfig>().FromInstance(playerConfig).AsSingle();
        }
    }
}