using System.Collections;
using System.Collections.Generic;
using Game.SpawnFallingObjects;
using Obstacles;
using UnityEngine;
using Zenject;

public class SpawnerFallingObject : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private List<Transform> _spawnPoints;
    
    [SerializeField] private FallObstacleFactory fallObstacleFactory;
    
    private LevelConfig _levelConfig;
    private IPlayer _player;
    private DynamicSpawnRateController _spawnRateController;
    private ResourceBalancer _balancer;
    
    private float _spawnTimeObstacle;
    
    private float _spawnTimeResource;
    
    private Vector3 _offset;
    private Coroutine _spawnObstacles;
    private Coroutine _spawnResources;
    
    [Inject] private void Construct(LevelConfig levelConfig, IPlayer player)
    {
        _levelConfig = levelConfig;
        _player = player;
        _spawnTimeObstacle = levelConfig.LevelDatas.StartTimeSpawmObstacle;
        _spawnTimeResource = levelConfig.LevelDatas.StartTimeSpawnResource;
    }

    private void Awake()
    {
        _offset.y = 2f * _camera.orthographicSize;
        
        _spawnRateController = new DynamicSpawnRateController(_levelConfig.LevelDatas.MaxHeightLevel);
        _balancer = new ResourceBalancer(_levelConfig);
    }

    private void OnEnable() =>
        EventAggregator.Subscribe<PlayeRanOutOfStaminaEventHandler>(OnRunOutStamina);

    private void OnDestroy() =>
        EventAggregator.Unsubscribe<PlayeRanOutOfStaminaEventHandler>(OnRunOutStamina);

    private void LateUpdate() =>
        UpdatePosition();
    
    private void UpdatePosition()
    {
        var updatedPosition = transform.position;
        updatedPosition.y = _camera.transform.position.y + _offset.y;
        transform.position = updatedPosition;
    }

    private IEnumerator SpawnObstacles()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnTimeObstacle);
            
            var randomNumberSpawmAmounObstaclest = UnityEngine.Random.Range(0, 2);
            SpawnObstacle(randomNumberSpawmAmounObstaclest);
            
            UpdateSpawnTimeObstacles();
        }
    }
    
    private IEnumerator SpawnResources()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnTimeResource);

            var spawnPoint = _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count - 1)];
            var categoryType = _balancer.GetRandomAvailableResourceCategory();
            
            fallObstacleFactory.Get(_levelConfig.ResourcesByCategory[categoryType], spawnPoint);

            UpdateSpawnTimeResources();
        }
    }

    private void SpawnObstacle(int amount)
    {
        for (int i = 0; i <= amount; i++)
        {
            var spawnPoint = _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count - 1)];

            var randomAvailableCategory = _balancer.GetRandomAvailableCategortType();
            fallObstacleFactory.Get(_levelConfig.ObstaclesByCategory[randomAvailableCategory], spawnPoint);
        }
    }

    private void UpdateSpawnTimeResources() =>
        _spawnTimeResource = _spawnRateController.CalculateAdjustedSpawnTime(
            _player.Transform.position.y,
            _levelConfig.LevelDatas.StartTimeSpawnResource,
            _levelConfig.LevelDatas.MinTimeSpawnResource);
    
    private void UpdateSpawnTimeObstacles() =>
        _spawnTimeObstacle = _spawnRateController.CalculateAdjustedSpawnTime(
            _player.Transform.position.y,
            _levelConfig.LevelDatas.StartTimeSpawmObstacle,
            _levelConfig.LevelDatas.MinTimeSpawObstacle);
    
    
    private void OnRunOutStamina(object sender, PlayeRanOutOfStaminaEventHandler eventData)
    {
        StopCoroutine(_spawnObstacles);
        StopCoroutine(_spawnResources);
    }
    
    private void OnTriggerEnter2D(Collider2D colider)
    {
        if (colider.CompareTag(ConstTags.StartSpawnObjects))
        {
            _spawnObstacles = StartCoroutine(SpawnObstacles());
            _spawnResources = StartCoroutine(SpawnResources());
        }

        if (colider.CompareTag(ConstTags.WinColider))
        {
            StopCoroutine(_spawnObstacles);
            StopCoroutine(_spawnResources);
        }

        if (colider.CompareTag(ConstTags.StopSpawnColider))
        {
            StopCoroutine(_spawnObstacles);
            StopCoroutine(_spawnResources);
        }
    }
}
