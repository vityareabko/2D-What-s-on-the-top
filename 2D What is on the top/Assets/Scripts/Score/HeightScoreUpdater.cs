using System;
using UI;
using UnityEngine;
using Zenject;

namespace Score
{
    public class HeightScoreUpdater : MonoBehaviour
    {
        [SerializeField] private Transform _character;
        
        private HeightTracker _heightTracker;
        private GameScreenHUDPresenter screenHUDPresenter;

        [Inject] public void Construct(GameScreenHUDPresenter hudPresenter)
        {
            screenHUDPresenter = hudPresenter;
            _heightTracker = new HeightTracker(_character.position.y);
        }

        private void Update()
        {
            screenHUDPresenter.UpdateHeightScore(_heightTracker.CalculateHeight(_character.position.y));
        }
    }
}