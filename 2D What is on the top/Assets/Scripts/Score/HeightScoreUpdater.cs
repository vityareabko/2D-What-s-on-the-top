using UI;
using UnityEngine;
using Zenject;

namespace Score
{
    public class HeightScoreUpdater : ITickable
    {
        private Transform _character;
        
        private HeightTracker _heightTracker;
        private GameScreenHUDPresenter screenHUDPresenter;

        [Inject] public void Construct(GameScreenHUDPresenter hudPresenter, IPlayer player)
        {
            screenHUDPresenter = hudPresenter;
            _character = player.Transform;
            _heightTracker = new HeightTracker(_character.position.y);
        }

        public void Tick() => screenHUDPresenter.UpdateHeightScore(_heightTracker.CalculateHeight(_character.position.y));
        
    }
}