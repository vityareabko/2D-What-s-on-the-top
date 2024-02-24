using UnityEngine;
using Zenject;

namespace Installers
{
    public class ConfigsGlobalInstaller : MonoInstaller
    {
        [SerializeField] private LevelDatabases _levelDatabases;
        [SerializeField] private CharacterData _characterData;
       
        
        public override void InstallBindings()
        {
            Container.Bind<CharacterData>().FromInstance(_characterData).AsSingle();
            Container.Bind<LevelDatabases>().FromInstance(_levelDatabases).AsSingle();
        }
    }
}