using System;
using TMPro;
using UI.MVP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public interface IGameSreenView : IView//<IGameScreenPresenter>
    {
        public void Initialize(StaminaData data);
        public void SetHightScore(int score);
        public void SetStaminaValue(float currentStamina);
    }

    public class GameScreenView : BaseScreenView, IGameSreenView
    {
        public override ScreenType ScreenType { get; } = ScreenType.GameScreen;
        
        [SerializeField] private TMP_Text _heightScoreText;
        [SerializeField] private Slider _stamina;
        
        public void Initialize(StaminaData data)
        {
            _stamina.minValue = data.MinStamina;
            _stamina.maxValue = data.MaxStamina;
            _stamina.value = _stamina.maxValue;
        }
        
        public void SetHightScore(int score) => _heightScoreText.text = $"{score.ToString()}m";
        
        public void SetStaminaValue(float currentStamina)
        {
            _stamina.value = currentStamina;
        }
    }
}