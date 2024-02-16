using Game.Gameplay;
using UnityEngine;
using Zenject;

namespace Installers.GameplayInstallers
{
    public class GameplayLevelInstaller : MonoInstaller
    {
        // [SerializeField] private GameplayController _gameplayController;
        [SerializeField] private Player _player;
        
        public override void InstallBindings()
        {
            // BindGameplayController();
            // BindPlayer();
            BindPayerStamin();
        }

        private void BindPayerStamin() => Container.Bind<Stamina>().AsSingle();
        
        //private void BindGameplayController() => Container.Bind<IGameplay>().FromInstance(_gameplayController).AsSingle();

        // private void BindPlayer() => Container.Bind<IPlayer>().To<Player>().FromInstance(_player).AsSingle();

    }
}