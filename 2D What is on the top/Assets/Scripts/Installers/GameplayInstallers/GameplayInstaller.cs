using System;
using System.Linq;
using Game.Gameplay;
using Levels;
using Obstacles;
using Score;
using Systems.SceneSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Installers.GameplayInstallers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private LevelsDB _levelsDB;
        
        [SerializeField] private Player _player;

        [SerializeField] private GameplayController _gameplayController;
        
        public override void InstallBindings()
        {

            BindGameplayController();
            BindIPlayer();
            BindPayerStamin();
            BindPlayerMover();
            BindLevelDB();
            BindScoreController();
            BindCameraStateMachine();
            BindHeightScoreUpdater();
        }
        private void BindPayerStamin() => Container.Bind<Stamina>().AsSingle();
        
        private void BindIPlayer()
        {
            var spawnPoint = _gameplayController.GetLevelSpawnPointByType(_levelsDB.CurrentLevel);
            Container.BindInterfacesAndSelfTo<Player>().FromComponentInNewPrefab(_player).UnderTransform(spawnPoint).AsSingle();
        }

        private void BindLevelDB() => Container.Bind<LevelsDB>().FromInstance(_levelsDB).AsSingle();
        
        private void BindHeightScoreUpdater() => Container.BindInterfacesAndSelfTo<HeightScoreUpdater>().AsSingle();
        
        private void BindScoreController() => Container.Bind<ScoreController>().AsSingle().NonLazy();

        private void BindPlayerMover() => Container.Bind<IPlayerMover>().To<PlayerMover>().AsSingle();
        
        private void BindGameplayController() => Container.Bind<GameplayController>().FromInstance(_gameplayController).AsSingle();
        
        private void BindCameraStateMachine()
        {
            var cameraSM = new CameraStateMaschine(CameraState.PlayerOnMainMenuPlatform);

            Container.Bind<ICameraStateMachine>().To<CameraStateMaschine>().FromInstance(cameraSM).AsSingle();
        }


    }
}