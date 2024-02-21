using Score;
using Scriptable.Datas.ConfigsLevel;
using UnityEngine;
using Zenject;

namespace Installers.GameplayInstallers
{
    public class GameplayLevelInstaller : MonoInstaller
    {
        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private Player _player;
        [SerializeField] private Transform _spawnPointPlayer;
        
        public override void InstallBindings()
        {
            BindIPlayer();
            BindPayerStamin();
            BindLevelConfig();
            BindScoreController();
            BindHeightScoreUpdater();
        }

        private void BindPayerStamin() => Container.Bind<Stamina>().AsSingle();

        private void BindIPlayer() => Container.Bind<IPlayer>().To<Player>().FromComponentInNewPrefab(_player).UnderTransform(_spawnPointPlayer).AsSingle();

        private void BindHeightScoreUpdater() => Container.BindInterfacesAndSelfTo<HeightScoreUpdater>().AsSingle();

        private void BindScoreController() => Container.Bind<ScoreController>().AsSingle().NonLazy();

        private void BindLevelConfig() => Container.Bind<LevelConfig>().FromInstance(_levelConfig).AsSingle();
    }
}