using System.Collections.Generic;
using Extensions;


namespace Game.SpawnFallingObjects
{
    public class ResourceBalancer
    {
        // # Todo - вынести это в конфиг, потому что у каждого уровня будет свои шансы выпадения рессурсов, особенно в уровнях за донат

        private const int _coins = 300;
        private const int _common = 30;
        private const int _rare = 20;
        private const int _epic = 10;
        private const int _legendary = 5;
        
        private  LevelConfig _levelConfig;
        
        private List<ResourceCategory> _resources = new();
        
        private List<ObstacleCategory> _obstacleCategories = new()
        {
            ObstacleCategory.BrownStone,
        };
        
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
            RemoveEmmptyObstacleCategories();
            InitializeResourcePool();
        }

        public ResourceCategory GetRandomAvailableResourceCategory() // стоит заменить random на более крутой рандом
        {
            var index = UnityEngine.Random.Range(0, _resources.Count);
            return _resources[index];
        }

        public ObstacleCategory GetRandomAvailableCategortType()
        {
            var index = UnityEngine.Random.Range(0, _obstacleCategories.Count);
            return _obstacleCategories[index];
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

        private void RemoveEmmptyObstacleCategories()
        {
            var availableCatecoriesObstacles = _levelConfig.ObstaclesByCategory;
        
            foreach (var key in availableCatecoriesObstacles)
            {
                if (key.Value.Count > 0)
                    continue;
        
                _obstacleCategories.Remove(key.Key);
            }
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