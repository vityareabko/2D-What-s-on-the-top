using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Levels;
using Systems.SceneSystem;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public class GameplayController : MonoBehaviour
    {
        [SerializeField] private List<LevelBehavior> _levels;

        private IPlayer _player;

        [Inject] private void Construct(IPlayer player, SceneController sceneController)
        {
            _player = player;
        }
        
        private void OnEnable()
        {
            EventAggregator.Subscribe<PauseGameEventHandler>(OnPauseGame);
            EventAggregator.Subscribe<ResumeGameEventHandler>(OnResumeGame);
        }

        private void OnDisable()
        {
            EventAggregator.Unsubscribe<PauseGameEventHandler>(OnPauseGame);
            EventAggregator.Unsubscribe<ResumeGameEventHandler>(OnResumeGame);
        }
        
        public Transform GetLevelSpawnPointByType(LevelType type) => _levels.First(t => t.Type == type).SpawnPoint;

        public void MovePlayerToLevelSpawnPoint(LevelType type)
        {
            StartCoroutine(MovePlayerCoroutine(type));
        }

        private void OnPauseGame(object sender, PauseGameEventHandler eventdata)
        {
            Time.timeScale = 0;
            EventAggregator.Post(this, new GameIsOnPausedEvent(){ IsOnPause = true });
        }

        private void OnResumeGame(object sender, ResumeGameEventHandler eventData)
        {
            Time.timeScale = 1;
            EventAggregator.Post(this, new GameIsOnPausedEvent() { IsOnPause = false });
        }
        
        private IEnumerator MovePlayerCoroutine(LevelType type)
        {
            var levelSpawnPoint = _levels.First(t => t.Type == type).SpawnPoint;
            _player.player.SetActive(false); 
            yield return null; 
            _player.Transform.position = levelSpawnPoint.position;
            _player.player.SetActive(true); 
        }

    }
}