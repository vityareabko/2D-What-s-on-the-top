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

    private Camera _camera;
    private IPlayer _target;
    private ICameraStateMachine _cameraStateMaschine;
    
    private Vector3 positionVelocity = Vector3.zero;
    private Vector3 positionChange = Vector3.zero;
    private Vector2 _OffsetVelocity;
    private Vector2 _currentoffset;
    private float _zoomVelocity = 0f;
    
    private bool _isStopFollowing = false;

    [Inject]
    private void Construct(IPlayer player, ICameraStateMachine cameraStateMaschine)
    {
        _target = player;
        _cameraStateMaschine = cameraStateMaschine;
    }

    private void Awake() => _camera = GetComponent<Camera>();

    private void LateUpdate()
    {
        if (_target == null || _isStopFollowing || Time.deltaTime == 0) 
            return;

        HandleCameraState();
    }

    private void HandleCameraState()
    {
        switch (_cameraStateMaschine.CurrentCameraState)
        {
            case CameraState.PlayerOnMainMenuPlatform:
                HandleMainMenuPlatform();
                break;
            case CameraState.PlayerOnPlatformLeft:
                HandleLeftPlatform();
                break;
            case CameraState.PlayerOnPlatformRight:
                HandleRightPlatform();
                break;
            case CameraState.PlayerIsNotOnThePlatform:
                HandleNotOnPlatform();
                break;
            case CameraState.PlayerLoseIsNotOnPlatform:
                HandleLoseNotOnPlatform();
                break;
            case CameraState.ShopSkinsMenu:
                HandleShopSkinsMenu();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void HandleMainMenuPlatform()
    {
        ZoomCamera(5f);
        
        SetSmoothlyOffset(0, 0);
    }

    private void HandleLeftPlatform()
    {
        SetSmoothlyOffset(_offsetCameraPlayerOnLeftPlatform, 0);
    }

    private void HandleRightPlatform()
    {
        SetSmoothlyOffset(_offsetCameraPlayerOnRightPlatform, 0);
    }

    private void HandleNotOnPlatform()
    {
        SetSmoothlyOffset(0, 0);
    }

    private void HandleLoseNotOnPlatform()
    {
        SetSmoothlyOffset(0, _offsetCameraPlayerLoseIsNotOnPlatform);
    }

    private void HandleShopSkinsMenu()
    {
        ZoomCamera(2f);
    }

    // Дополнительные методы для работы с камерой
    private void SetSmoothlyOffset(float offsetX, float offsetY)
    {
        _currentoffset.x = Mathf.SmoothDamp(_currentoffset.x, offsetX, ref _OffsetVelocity.x, SmoothTime);
        _currentoffset.y = Mathf.SmoothDamp(_currentoffset.y, offsetY, ref _OffsetVelocity.y, SmoothTime);
        ApplyCameraPosition();
    }

    private void ApplyCameraPosition()
    {
        Vector3 desiredPosition = _target.Transform.position + new Vector3(_currentoffset.x, _offset.y, _offset.z);
        Vector3 newPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref positionChange, SmoothTime);
        transform.position = StabilizeCameraPosition(newPosition);
    }

    private void ZoomCamera(float targetOrthographicSize)
    {
        float smoothTime = 0.08f;
        
        // Todo: - при переходе из ShopSkins в MainMenu - то быстро смещается камера нужно бы поисправить
        
        _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, targetOrthographicSize, ref _zoomVelocity, smoothTime);
        transform.position = Vector3.SmoothDamp(transform.position, _offset, ref positionVelocity, smoothTime);
    }
    

    private Vector3 StabilizeCameraPosition(Vector3 newPosition)
    {
        Vector3 velocity = (newPosition - transform.position) / Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, _maxPositionChangePerFrame);
        
        var newPos = transform.position + velocity * Time.deltaTime;

        newPos.y = _target.Transform.position.y + _currentoffset.y;
        return newPos;
    }
}