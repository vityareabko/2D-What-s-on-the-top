using Obstacles;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class ConfigsGlobalInstaller : MonoInstaller
    {
        [SerializeField] private CharacterData _characterData;
       
        public override void InstallBindings()
        {
            Container.Bind<CharacterData>().FromInstance(_characterData).AsSingle();
        }
    }
}