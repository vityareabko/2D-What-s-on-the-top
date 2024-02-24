using Score;
using UnityEngine;
using Zenject;

namespace Installers.GameplayInstallers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private Player _player;
        [SerializeField] private Transform _spawnPointPlayer;

        private LevelConfig _levelConfig; // # todo - поидея должен прокидыватся потому что при переходе я закидываю конфиг уровня в контайнер но хз
        
        [Inject]
        public void Construct(LevelConfig levelConfig)
        {
            _levelConfig = levelConfig;
            Debug.Log(_levelConfig);
        }
        
        public override void InstallBindings()
        {
            BindIPlayer();
            BindPayerStamin();
            BindScoreController();
            BindHeightScoreUpdater();
            
        }

        private void BindPayerStamin() => Container.Bind<Stamina>().AsSingle();

        private void BindIPlayer() => Container.Bind<IPlayer>().To<Player>().FromComponentInNewPrefab(_player).UnderTransform(_spawnPointPlayer).AsSingle();

        private void BindHeightScoreUpdater() => Container.BindInterfacesAndSelfTo<HeightScoreUpdater>().AsSingle();

        private void BindScoreController() => Container.Bind<ScoreController>().AsSingle().NonLazy();
    }
}