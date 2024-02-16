using System;
using System.Collections;
using System.Collections.Generic;
using Obstacles;
using UnityEngine;
using UnityEngine.Serialization;

public class FallingObstacleSpawner : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private List<Transform> _spawnPoints;
    
    [FormerlySerializedAs("_obstacleFactory")] [SerializeField] private FallObstacleFactory fallObstacleFactory;

    private List<FallObstacle> _obstacles = new();
    
    private Vector3 _offset;

    private Coroutine _spawnObstacles;
    
    private void Awake()
    {
        _spawnObstacles = StartCoroutine(SpawnObstacles());
        _offset.y = 2f * _camera.orthographicSize;
    }

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
            yield return new WaitForSeconds(1f);
            var instance = fallObstacleFactory.Get((FallingObstaclesType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(FallingObstaclesType)).Length),
            _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Count - 1)]);
        
            _obstacles.Add(instance);
        }
    }

    private void OnTriggerEnter2D(Collider2D colider)
    {
        if (colider.CompareTag(ConstTags.WinColider))
            // остановка спавна
            _spawnObstacles = StartCoroutine(SpawnObstacles());

        if (colider.CompareTag(ConstTags.StopSpawnObstaclesColider))
            // остановка спавна
            _spawnObstacles = StartCoroutine(SpawnObstacles());
    }
}
