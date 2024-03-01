using System;
using System.Linq;
using Score;
using UnityEngine;
using Zenject;

namespace Installers.GameplayInstallers
{
    [Serializable]
    public class SpawnPointsData
    {
        [field: SerializeField] public SpawnPointType Type {get; private set;}
        [field: SerializeField] public Transform SpawnPoint {get; private set;}
    }

    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private Player _player;
        
        [SerializeField] private Transform _spawnPointPlayer;
        
        // здесь scriptableObject где хранится PlayerConfig
        [SerializeField] private PlayerConfig _playerConfig;

        [SerializeField] private SpawnPointsData[] _spawnPoints; 
        
        
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
        private void BindIPlayer()
        {
            // получаем тип spawn point из PlayerConfig
            var spawnPoint = GetSpawnPoint(_playerConfig.CurrentSpawnPoint);
            
            // Container.BindInterfacesAndSelfTo<Player>().FromComponentInNewPrefab(_player).UnderTransform(_spawnPointPlayer).AsSingle();
            Container.BindInterfacesAndSelfTo<Player>().FromComponentInNewPrefab(_player).UnderTransform(spawnPoint).AsSingle();
            
        }

        private void BindLevelConfig() => Container.Bind<LevelConfig>().FromInstance(_levelConfig).AsSingle();
        private void BindHeightScoreUpdater() => Container.BindInterfacesAndSelfTo<HeightScoreUpdater>().AsSingle();
        private void BindScoreController() => Container.Bind<ScoreController>().AsSingle().NonLazy();

        private void BindPlayerMover() => Container.Bind<IPlayerMover>().To<PlayerMover>().AsSingle();
        
        private void BindCameraStateMachine()
        {
            var cameraSM = new CameraStateMaschine(CameraState.PlayerOnMainMenuPlatform);

            Container.Bind<ICameraStateMachine>().To<CameraStateMaschine>().FromInstance(cameraSM).AsSingle();
        }
        
        // метод который возвращает transform по тип спавна точки
        private Transform GetSpawnPoint(SpawnPointType type) => _spawnPoints.First(t => t.Type == type).SpawnPoint;

    }
}