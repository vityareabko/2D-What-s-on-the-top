using System;
using System.Collections.Generic;
using System.Linq;
using Scriptable.Datas.FallResources;
using UnityEngine;

namespace Obstacles
{
    [CreateAssetMenu(fileName = "FallObstacleFactory", menuName = "Factory/FallObject")]
    public class FallObstacleFactory : ScriptableObject
    {
        [SerializeField] private FallObstacleDatabase _obstacleDatabase;

        public FallObject Get(FallingObstaclesType type, Transform parent)
        {
            var cofig = _obstacleDatabase.FallObstacleConfigs.First(t => t.Type == type);
            var instance = Instantiate(cofig.Prefab, parent);
            instance.Initialize(cofig.Speed);
            
            return instance;
        }

        public FallObject Get(FallingResourceType type, Transform parent)
        {
            var cofig = _obstacleDatabase.FallingResourceConfigs.First(t => t.Type == type);
            var instance = Instantiate(cofig.Prefab, parent);
            instance.Initialize(cofig.Speed);
            
            return instance;
        }
    }
}