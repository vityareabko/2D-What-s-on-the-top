using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public class ObstaclleController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _obstacles;

        private void OnEnable()
        {
            EventAggregator.Subscribe<PlayerLoseHideAllObstaclesEvent>(OnHideAllObstacles);
        }

        private void OnDestroy()
        {
            EventAggregator.Unsubscribe<PlayerLoseHideAllObstaclesEvent>(OnHideAllObstacles);
        }

        private void OnHideAllObstacles(object sender, PlayerLoseHideAllObstaclesEvent eventData)
        {
            foreach (var obstacle in _obstacles)
            {
                obstacle.gameObject.SetActive(false);
            }
        }
        
    }
}