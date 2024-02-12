using System;
using TriggersScripts;
using UI;
using UnityEngine;

namespace Game.Gameplay
{
    public interface IGameplay
    {
        public event Action PlayerWin;
        public event Action PlayerDefeat;
    }

    public class GameplayController : MonoBehaviour, IGameplay
    {
        public event Action PlayerWin;
        public event Action PlayerDefeat;

        [SerializeField] private PlayerController _playerController;
        [SerializeField] private TriggerWinLevel _triggerWinLevel;
        
        
        private void OnEnable()
        {
            _triggerWinLevel.LevelWin += OnLevelWinHandler;
            _playerController.CharacterDefeat += OnLevelDefeatHandler;
        }

        private void OnDisable()
        {
            _triggerWinLevel.LevelWin -= OnLevelWinHandler;
            _playerController.CharacterDefeat -= OnLevelDefeatHandler;
        }

        public void PauseGame()
        {
            Time.timeScale = 0;
            _playerController.BlockSwipe(false);
        }

        public void ResumeGame()
        {
            Time.timeScale = 1;
            _playerController.BlockSwipe(true);
        }

        private void OnLevelDefeatHandler()
        {
            // сдесь в action можно передать кол-во заработаных монет или еще что-то чтобы передать въюхе эти данные
            PlayerDefeat?.Invoke();
        }
        
        private void OnLevelWinHandler()
        {
            // сдесь по-хорошому надо бы передать награды за уровенеь которые будут братся из конфига к примеру за прохождения 1-ого уровня дают 1000 монет и еще что-то
            PlayerWin?.Invoke();
        }

    }
}