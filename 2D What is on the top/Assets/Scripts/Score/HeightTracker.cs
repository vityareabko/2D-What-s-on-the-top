using System;
using UnityEngine;

namespace Score
{
    public class HeightTracker
    {
        public event Action<int> HeightTrackerChange;

        private float _currentHeight;

        public HeightTracker(float currentHeight)
        {
            _currentHeight = currentHeight + 1;
        }

        public void CalculateHeight(float amount)
        {
            if (_currentHeight > amount)
                return;
            
            _currentHeight = amount;
            
            var heigh = Mathf.FloorToInt(amount);
            HeightTrackerChange?.Invoke(heigh);
        }

        public int GetCurrentHeight() => Mathf.FloorToInt(_currentHeight);
    }
}