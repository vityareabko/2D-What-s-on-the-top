using System;
using UnityEngine;
using Zenject;

public class FollowCamera : MonoBehaviour
{
    private const float _offsetCameraFromPlayerToRight = 1.5f;
    private const float _offsetCameraFromPlayerToLeft = -1.5f;
    
    [SerializeField] private Vector3 _offset = new Vector3(1, 0, -15);
    [SerializeField] private float _maxPositionChangePerFrame = 0.5f;
    
    private IPlayer _target;
    
    private Vector3 positionChange = Vector3.zero;
    private float _currentXOffset;
    private float _xOffsetVelocity; // Для хранения текущей скорости изменения смещения.
    
    private bool _isStopFollowing = false;

    [Inject]
    private void Construct(IPlayer player) => _target = player;
    
    private void Start() => _offset.x = _offsetCameraFromPlayerToLeft;
    
    private void OnEnable()
    {
        EventAggregator.Subscribe<PlayerJumpedToAgainsWallEvent>(OnPlayerJumped);
        // EventAggregator.Subscribe<PlayerLoseEventHandler>(OnPlayerLose);
    }

    private void OnDisable()
    {
        EventAggregator.Unsubscribe<PlayerJumpedToAgainsWallEvent>(OnPlayerJumped);
        // EventAggregator.Unsubscribe<PlayerLoseEventHandler>(OnPlayerLose);
    }
    
    private void OnPlayerJumped(object sender, PlayerJumpedToAgainsWallEvent eventData) =>
        _offset.x = eventData.IsRightWall ? _offsetCameraFromPlayerToLeft : _offsetCameraFromPlayerToRight;
    

    private void LateUpdate()
    {
        if (_target == null || _isStopFollowing || Time.deltaTime == 0) 
            return;
        
        float smoothTime = 0.5f; // Время плавного перехода
            
            
        if (_target.IsOnPlatform)
            _currentXOffset = Mathf.SmoothDamp(_currentXOffset, _offset.x, ref _xOffsetVelocity, smoothTime);
        else
            _currentXOffset = Mathf.SmoothDamp(_currentXOffset, 0, ref _xOffsetVelocity, smoothTime);

        // Плавное изменение текущего смещения по оси X до нового значения.

        Vector3 desiredPosition = _target.Transform.position + new Vector3(_currentXOffset, _offset.y, _offset.z);

        // Вычисляем новую позицию с учетом плавного перехода
        Vector3 newPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref positionChange, smoothTime);

        // Ограничиваем максимальную скорость изменения позиции камеры, чтобы избежать резких скачков
        Vector3 velocity = (newPosition - transform.position) / Time.deltaTime; // Вычисляем скорость
        velocity = Vector3.ClampMagnitude(velocity, _maxPositionChangePerFrame); // Ограничиваем скорость
        newPosition = transform.position + velocity * Time.deltaTime; // Применяем ограниченную скорость для вычисления новой позиции

        newPosition.y = _target.Transform.position.y;
        
        transform.position = newPosition;
    }

// Убедитесь, что у вас есть переменная для positionChange, если будете использовать SmoothDamp для позиции

    // private Vector3 AdjustPositionWithinBounds(Vector3 position)
    // {
    //     // Здесь задайте границы для вашего игрового мира или сцены
    //     float minX = -1f; // Пример минимальной границы по X
    //     float maxX = 1f;  // Пример максимальной границы по X
    //
    //     // Корректировка позиции, чтобы она оставалась внутри этих границ
    //     position.x = Mathf.Clamp(position.x, minX, maxX);
    //
    //     return position;
    // }
    //
    // private float GeneratePerlinShake()
    // {
    //     float shakeMagnitude = 1f;
    //     float noiseSpeed = 0.5f;
    //     return Mathf.PerlinNoise(Time.time * noiseSpeed, 0) * shakeMagnitude - shakeMagnitude / 2;
    // }
    
    // private void OnPlayerDefeat() => _isStopFollowing = true;
    // private void OnPlayerWin() => _isStopFollowing = true;
    
    // private void OnPlayerLose(object arg1, PlayerLoseEventHandler arg2) => _isStopFollowing = true;

}
