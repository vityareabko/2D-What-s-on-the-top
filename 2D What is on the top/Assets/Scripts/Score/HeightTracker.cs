using System;
using UnityEngine;

namespace Score
{
    public class HeightTracker
    {
        private float _currentHeight;

        public HeightTracker(float currentHeight)
        {
            _currentHeight = currentHeight;
        }

        public int CalculateHeight(float amount)
        {
            if (_currentHeight > amount)
                return Mathf.FloorToInt(_currentHeight);

            _currentHeight = amount;
            return Mathf.FloorToInt(amount);
        }
    }
}