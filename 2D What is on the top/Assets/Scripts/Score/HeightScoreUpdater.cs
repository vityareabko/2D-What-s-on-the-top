using System;
using UI;
using UnityEngine;
using Zenject;

namespace Score
{
    public class HeightScoreUpdater : ITickable, IDisposable
    {
        private Transform _character;
        
        private HeightTracker _heightTracker;
        private GameScreenHUDPresenter screenHUDPresenter;

        private bool _stopUpdateTracker;
        
        [Inject] public void Construct(GameScreenHUDPresenter hudPresenter, IPlayer player)
        {
            screenHUDPresenter = hudPresenter;
            _character = player.Transform;
            _heightTracker = new HeightTracker(_character.position.y);
            
            EventAggregator.Subscribe<SwitchGameStateToLoseGameEvent>(OnPlayerLoseStopUpdateTracker);
        }

        public void Dispose() => EventAggregator.Unsubscribe<SwitchGameStateToLoseGameEvent>(OnPlayerLoseStopUpdateTracker);

        public void Tick()
        {
            if (_stopUpdateTracker)
                return;
            
            screenHUDPresenter.UpdateHeightScore(_heightTracker.CalculateHeight(_character.position.y));
        }

        private void OnPlayerLoseStopUpdateTracker(object sender, SwitchGameStateToLoseGameEvent eventData) => _stopUpdateTracker = true;

    }
}