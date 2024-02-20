using System;
using System.Collections;
using System.Collections.Generic;
using Obstacles;
using Scriptable.Datas.FallResources;
using UnityEngine;

public class FallingObjectSpawner : MonoBehaviour
{
    // TODO - сделать систему которая будет следить за процентом прохождения уровня а и постепенно увеличивать скорость спавна объяектов и увеличивать минимальную скорость падения препятсвий в рандоме
    
    // Todo - нужно сделать систему вероятносити выпадения у рессурсов;
    
    [SerializeField] private Camera _camera;
    [SerializeField] private List<Transform> _spawnPoints;
    
    [SerializeField] private FallObstacleFactory fallObstacleFactory;

    private List<FallObject> _obstacles = new();
    
    private Vector3 _offset;

    private Coroutine _spawnObstacles;
    private Coroutine _spawnResources;
    
    private void Awake()
    {
        _spawnObstacles = StartCoroutine(SpawnObstacles());
        _spawnResources = StartCoroutine(SpawnResources());
        _offset.y = 2f * _camera.orthographicSize;
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
            yield return new WaitForSeconds(.5f);
            var instance = fallObstacleFactory.Get((FallingObstaclesType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(FallingObstaclesType)).Length),
            _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count - 1)]);
        
            _obstacles.Add(instance);
        }
    }
    
    private IEnumerator SpawnResources()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            var instance = fallObstacleFactory.Get((FallingResourceType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(FallingResourceType)).Length),
            _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count - 1)]);
        
            _obstacles.Add(instance);
        }
    }
    
    private void OnRunOutStamina(object sender, PlayeRanOutOfStaminaEventHandler eventData)
    {
        StopCoroutine(_spawnObstacles);
        StopCoroutine(_spawnResources);
    }

    private void OnTriggerEnter2D(Collider2D colider)
    {
        if (colider.CompareTag(ConstTags.WinColider))
        {
            StopCoroutine(_spawnObstacles);
            StopCoroutine(_spawnResources);
        }

        if (colider.CompareTag(ConstTags.StopSpawnObstaclesColider))
        {
            StopCoroutine(_spawnObstacles);
            StopCoroutine(_spawnResources);
        }
    }
}
