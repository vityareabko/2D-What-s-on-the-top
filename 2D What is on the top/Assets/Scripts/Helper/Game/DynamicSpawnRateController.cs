using UnityEngine;

namespace Game.SpawnFallingObjects
{
    public class DynamicSpawnRateController
    {
        private const float ProgressAmplificationFactor = 1.3f;
        
        private int _maxHeight;
        
        public DynamicSpawnRateController(int maxHeight) => _maxHeight = maxHeight;
        
        public float CalculateAdjustedSpawnTime(float currentHeight, float _startSpawnTime, float _minSpawnTime)
        {
            var progress =  (currentHeight * ProgressAmplificationFactor) / _maxHeight; // где-то на 75-80% будт уже идты _minSpawnTime;

            var spawnTime = _startSpawnTime - (progress * (_startSpawnTime - _minSpawnTime));

            return Mathf.Max(spawnTime, _minSpawnTime);
        }
    }
}