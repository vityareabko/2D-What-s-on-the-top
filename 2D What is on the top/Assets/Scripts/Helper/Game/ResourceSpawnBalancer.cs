using System.Collections.Generic;
using Obstacles;

namespace Game.SpawnFallingObjects
{
    public class ResourceSpawnBalancer
    {
        // # Todo - вынести это в конфиг, потому что у каждого уровня будет свои шансы выпадения рессурсов, особенно в уровнях за донат
        
        
        private const int _common = 50;
        private const int _rare = 20;
        private const int _epic = 10;
        private const int _legendary = 2;

        private List<ResourceCategory> _resources = new();
        
        private Dictionary<ResourceCategory, int> _weights = new()
        {
            { ResourceCategory.Common, _common },
            { ResourceCategory.Rare , _rare} ,
            { ResourceCategory.Epic, _epic },
            { ResourceCategory.Legendary, _legendary },
        };

        public ResourceSpawnBalancer()
        {
            InitializeResourcePool();
            InitializeWeights();
        }

        public ResourceCategory GetRandomResourceType() // стоит заменить random на более крутой рандом
        {
            var index = UnityEngine.Random.Range(0, _resources.Count);
            return _resources[index];
        }

        private void InitializeWeights()
        {
            
        }

        private void InitializeResourcePool()
        {
            foreach (var item in _weights)
                for (int i = 0; i < item.Value; i++)
                    _resources.Add(item.Key);
        }
        
        
    }
}