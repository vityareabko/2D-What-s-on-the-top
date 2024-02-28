using System;
using UnityEngine;
using Zenject;

public class FollowCamera : MonoBehaviour
{
    private const float _offsetCameraPlayerOnLeftPlatform = 1.5f;
    private const float _offsetCameraPlayerOnRightPlatform = -1.5f;
    private const float _offsetCameraPlayerLoseIsNotOnPlatform = 3f;
    
    private const float SmoothTime = 0.3f; 
    
    [SerializeField] private Vector3 _offset = new Vector3(0, 0, -15);
    
    [SerializeField] private float _maxPositionChangePerFrame = 0.5f;
    
    private IPlayer _target;
    private ICameraStateMachine _cameraStateMaschine;
    
    private Vector3 positionChange = Vector3.zero;
    private Vector2 _OffsetVelocity;
    private Vector2 _currentoffset;
    
    private bool _isStopFollowing = false;

    [Inject]
    private void Construct(IPlayer player, ICameraStateMachine cameraStateMaschine)
    {
        _target = player;
        _cameraStateMaschine = cameraStateMaschine;
    }

    private void Start() => _offset.x = _offsetCameraPlayerOnRightPlatform;
    
    // private void Update()
    // {
    //     if(Input.GetKeyDown(KeyCode.Q))
    //         EventAggregator.Post(this, new SwitchCameraStateOnMainMenuPlatform());
    //     if(Input.GetKeyDown(KeyCode.W))
    //         EventAggregator.Post(this, new SwitchCameraStateOnPlayerLeftPlatform());
    //     if(Input.GetKeyDown(KeyCode.E))
    //         EventAggregator.Post(this, new SwitchCameraStateOnPlayerRightPlatform());
    //     if(Input.GetKeyDown(KeyCode.R))
    //         EventAggregator.Post(this, new SwitchCameraStateOnPlayerLoseIsNotOnPlatform());
    //     if(Input.GetKeyDown(KeyCode.T))
    //         EventAggregator.Post(this, new SwitchCameraStateOnPlayerIsNotOnThePlatform());
    //     
    // }

    private void LateUpdate()
    {
        if (_target == null || _isStopFollowing || Time.deltaTime == 0) 
            return;

        ObserveCameraCurrentState();

        // Плавное изменение текущего смещения по оси X до нового значения.

        Vector3 desiredPosition = _target.Transform.position + new Vector3(_currentoffset.x, _offset.y, _offset.z);

        // Вычисляем новую позицию с учетом плавного перехода
        Vector3 newPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref positionChange, SmoothTime);
        
        newPosition = StabilizeCameraPosition(newPosition);

        newPosition.y = _target.Transform.position.y + _currentoffset.y; 
        
        transform.position = newPosition;
    }

    private Vector3 StabilizeCameraPosition(Vector3 newPosition)
    {
        // Ограничиваем максимальную скорость изменения позиции камеры, чтобы избежать резких скачков
        Vector3 velocity = (newPosition - transform.position) / Time.deltaTime; // Вычисляем скорость
        velocity = Vector3.ClampMagnitude(velocity, _maxPositionChangePerFrame); // Ограничиваем скорость
        newPosition = transform.position + velocity * Time.deltaTime; // Применяем ограниченную скорость для вычисления новой позиции
        
        return newPosition;
    }

    private void ObserveCameraCurrentState()
    {
        Debug.Log(_cameraStateMaschine.CurrentCameraState);
        switch (_cameraStateMaschine.CurrentCameraState)
        {
            case CameraState.PlayerOnMainMenuPlatform:
                SetSmoothlyOffset(0, 0);
                break;
            case CameraState.PlayerOnPlatformLeft:
                SetSmoothlyOffset(_offsetCameraPlayerOnLeftPlatform, 0);
                break;
            case CameraState.PlayerOnPlatformRight:
                SetSmoothlyOffset(_offsetCameraPlayerOnRightPlatform, 0);
                break;
            case CameraState.PlayerIsNotOnThePlatform:
                SetSmoothlyOffset(0, 0);
                break;
            case CameraState.PlayerLoseIsNotOnPlatform:
                SetSmoothlyOffset(0, _offsetCameraPlayerLoseIsNotOnPlatform);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetSmoothlyOffset (float offsetX, float offsetY)
    {
        _currentoffset.x = Mathf.SmoothDamp(_currentoffset.x, offsetX, ref _OffsetVelocity.x, SmoothTime);
        _currentoffset.y = Mathf.SmoothDamp(_currentoffset.y, offsetY, ref _OffsetVelocity.y, 1f);
    }
}
