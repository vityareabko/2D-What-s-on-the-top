using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Obstacles
{
    [CreateAssetMenu(fileName = "FallObstacleFactory", menuName = "Factory/FallObstacle")]
    public class FallObstacleFactory : ScriptableObject
    {
        [SerializeField] private FallObstacleDatabase _obstacleDatabase;

        public FallObstacle Get(FallingObstaclesType type, Transform parent)
        {
            var cofig = _obstacleDatabase.FallObstacleConfigs.First(t => t.Type == type);
            var instance = Instantiate(cofig.Prefab, parent);
            instance.Initialize(cofig.Speed);
            
            return instance;
        }
    }
}