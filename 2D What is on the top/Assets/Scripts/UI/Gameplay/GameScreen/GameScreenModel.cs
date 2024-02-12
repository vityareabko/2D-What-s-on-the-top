using System;
using UI.MVP;

namespace UI
{
    public interface IGameScreenModel : IModel
    {
        public event Action<int> HightScoreChange;
        public event Action<float> StaminaChange;

        public int Score { get; set; }
        public float Stamina { get; set; }
    }

    public class GameScreenModel : IGameScreenModel
    {
        public event Action<float> StaminaChange;
        public event Action<int> HightScoreChange;
        
        private int _hightScore;
        private float _stamina;

        public int Score 
        { 
            get => _hightScore;
            set
            {
                _hightScore = value;
                HightScoreChange?.Invoke(_hightScore);
            }
        }
        
        public float Stamina 
        {
            get => _stamina;
            set
            {
                _stamina = value;
                StaminaChange?.Invoke(_stamina);
            }
        }
    }
}