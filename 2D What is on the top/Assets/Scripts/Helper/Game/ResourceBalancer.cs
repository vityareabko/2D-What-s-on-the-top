using System.Collections.Generic;
using System.Linq;
using Extensions;
using ModestTree;
using Obstacles;

namespace Game.SpawnFallingObjects
{
    public class ResourceBalancer
    {
        // # Todo - вынести это в конфиг, потому что у каждого уровня будет свои шансы выпадения рессурсов, особенно в уровнях за донат

        private const int _coins = 130;
        private const int _common = 30;
        private const int _rare = 20;
        private const int _epic = 10;
        private const int _legendary = 5;

        private List<ResourceCategory> _resources = new();
        
        private  LevelConfig _levelConfig;
        
        private Dictionary<ResourceCategory, int> _weights = new()
        {
            { ResourceCategory.Common, _common },
            { ResourceCategory.Rare , _rare} ,
            { ResourceCategory.Epic, _epic },
            { ResourceCategory.Legendary, _legendary },
            { ResourceCategory.Coin , _coins},
        };

        public ResourceBalancer(LevelConfig levelConfig)
        {
            _levelConfig = levelConfig;
            RemoveEmptyResourceCategoriesFromWeights();
            InitializeResourcePool();
        }

        private void RemoveEmptyResourceCategoriesFromWeights()
        {
            var availableCategoriesWithResources = _levelConfig.ResourcesByCategory;
            
            foreach (var key in availableCategoriesWithResources)
            {
                if (key.Value.Count > 0)
                    continue;

                _weights.Remove(key.Key);
            }        
        }

        public ResourceCategory GetRandomResourceType() // стоит заменить random на более крутой рандом
        {
            var index = UnityEngine.Random.Range(0, _resources.Count);
            return _resources[index];
        }
        
        private void InitializeResourcePool()
        {
            foreach (var item in _weights)
                for (int i = 0; i < item.Value; i++)
                    _resources.Add(item.Key);

            _resources.Shuffle();
        }
        
        
    }
}