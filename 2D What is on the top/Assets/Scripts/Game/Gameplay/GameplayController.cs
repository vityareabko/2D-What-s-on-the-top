using System;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public interface IGameplay
    {
        public event Action PlayerWin;
        public event Action PlayerDefeat;
        public void PauseGame();
        public void ResumeGame();
    }

    public class GameplayController : MonoBehaviour, IGameplay
    {
        public event Action PlayerWin;
        public event Action PlayerDefeat;

        private IPlayer _player;
        
        [Inject] private void Construct(IPlayer player)
        {
            _player = player;
        }

        private void OnEnable()
        {
            _player.LevelWin += OnLevelWinHandler;
            _player.LevelDefeat += OnLevelDefeatHandler;
        }
        
        private void OnDisable()
        {
            _player.LevelWin -= OnLevelWinHandler;
            _player.LevelDefeat -= OnLevelDefeatHandler;
        }

        public void PauseGame()
        {
            Time.timeScale = 0;
            _player.BlockSwipe(true);
        }

        public void ResumeGame()
        {
            Time.timeScale = 1;
            _player.BlockSwipe(false);
        }

        private void OnLevelDefeatHandler()
        {
            PlayerDefeat?.Invoke();
            _player.BlockSwipe(true);
        }
        
        private void OnLevelWinHandler()
        {
            PlayerWin?.Invoke();
            _player.BlockSwipe(true);
        }
    }
}