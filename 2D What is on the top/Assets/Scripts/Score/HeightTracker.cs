using UnityEngine;

namespace Score
{
    public class HeightTracker
    {
        private Transform _character;
        private float _startHeght;

        public HeightTracker(Transform character, float startHeght)
        {
            _character = character;
            _startHeght = startHeght;
        }

        public int CalculateHeight() => Mathf.FloorToInt(_character.position.y);
    }
}