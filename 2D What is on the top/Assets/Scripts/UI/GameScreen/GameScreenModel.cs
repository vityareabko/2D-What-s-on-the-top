using System;
using UI.MVP;
using UnityEngine;

namespace UI
{
    public interface IGameScreenModel : IModel
    {
        public event Action<int> ScoreChange;
        public int Score { get; }
    }

    public class GameScreenModel : IGameScreenModel
    {
        public event Action<int> ScoreChange;
        
        private int _score;

        public GameScreenModel(int score)
        {
            _score = score;
        }

        public int Score 
        { 
            get => _score;
            set
            {
                _score = value;
                Debug.Log("event next step");
                ScoreChange?.Invoke(_score);
            }
        }
        
        // Метод для добавления очков к текущему счёту
        public void AddScore(int amount)
        {
            Score += amount; // Это автоматически вызовет ScoreChange?.Invoke(_score);
        }
    }
}