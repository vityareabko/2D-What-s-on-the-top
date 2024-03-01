using Score;
using UnityEngine;
using Zenject;

namespace Installers.GameplayInstallers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private Player _player;
        [SerializeField] private Transform _spawnPointPlayer;
        
        public override void InstallBindings()
        {
            BindIPlayer();
            BindPayerStamin();
            BindPlayerMover();
            BindLevelConfig();
            BindScoreController();
            BindCameraStateMachine();
            BindHeightScoreUpdater();
        }
        private void BindPayerStamin() => Container.Bind<Stamina>().AsSingle();
        private void BindIPlayer() => Container.BindInterfacesAndSelfTo<Player>().FromComponentInNewPrefab(_player).UnderTransform(_spawnPointPlayer).AsSingle();
        
        private void BindLevelConfig() => Container.Bind<LevelConfig>().FromInstance(_levelConfig).AsSingle();
        private void BindHeightScoreUpdater() => Container.BindInterfacesAndSelfTo<HeightScoreUpdater>().AsSingle();
        private void BindScoreController() => Container.Bind<ScoreController>().AsSingle().NonLazy();

        private void BindPlayerMover() => Container.Bind<IPlayerMover>().To<PlayerMover>().AsSingle();
        
        private void BindCameraStateMachine()
        {
            var cameraSM = new CameraStateMaschine(CameraState.PlayerOnMainMenuPlatform);

            Container.Bind<ICameraStateMachine>().To<CameraStateMaschine>().FromInstance(cameraSM).AsSingle();
        }
    }
}