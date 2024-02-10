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
        private GameScreenPresenter _screenPresenter;

        [Inject] public void Construct(GameScreenPresenter presenter)
        {
            _screenPresenter = presenter;
            _heightTracker = new HeightTracker(_character.position.y);
        }

        private void Update()
        {
            _screenPresenter.UpdateHeightScore(_heightTracker.CalculateHeight(_character.position.y));
        }
    }
}