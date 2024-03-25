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

    private Camera _camera;
    private IPlayer _target;
    private ICameraStateMachine _cameraStateMaschine;
    
    private CameraState _previousCameraState = CameraState.PlayerOnMainMenuPlatform; 
    
    private Vector3 _positionVelocity = Vector3.zero;
    private Vector3 _positionChange = Vector3.zero;
    private Vector2 _OffsetVelocity;
    private Vector2 _currentoffset;
    private float _zoomVelocity;
    
    private bool _isStopFollowing = false;

    [Inject] private void Construct(IPlayer player, ICameraStateMachine cameraStateMaschine)
    {
        _target = player;
        _cameraStateMaschine = cameraStateMaschine;
    }
    
    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _camera.transform.position = new Vector3(_target.Transform.position.x, _target.Transform.position.y);
    }

    private void LateUpdate()
    {
        if (_target == null || _isStopFollowing || Time.deltaTime == 0) 
            return;
        
        if (_cameraStateMaschine.CurrentCameraState != _previousCameraState) {
            _previousCameraState = _cameraStateMaschine.CurrentCameraState;
            ResetOffset(); // Сброс или обновление offset при смене состояния
        }

        float yDifference = _target.Transform.position.y - _camera.transform.position.y;
        
        if (Mathf.Abs(yDifference) > 50) 
            _camera.transform.position = new Vector3(_camera.transform.position.x, _target.Transform.position.y);

        HandleCameraState(); 
        Debug.Log(_cameraStateMaschine.CurrentCameraState);
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
    
    private void ResetOffset() 
    {
        _currentoffset = Vector2.zero; 
        _OffsetVelocity = Vector2.zero;
    }

    private void HandleMainMenuPlatform()
    {
        ZoomCamera(5f, new Vector3(0,_target.Transform.position.y,-15));
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
        ZoomCamera(2.5f, new Vector3(.5f, _target.Transform.position.y + 0.5f, -15f));
    }
    
    private void SetSmoothlyOffset(float offsetX, float offsetY)
    {
        _currentoffset.x = Mathf.SmoothDamp(_currentoffset.x, offsetX, ref _OffsetVelocity.x, SmoothTime);
        _currentoffset.y = Mathf.SmoothDamp(_currentoffset.y, offsetY, ref _OffsetVelocity.y, SmoothTime);
        ApplyCameraPosition();
    }

    private void ApplyCameraPosition()
    {
        Vector3 desiredPosition = _target.Transform.position + new Vector3(_currentoffset.x, _offset.y, _offset.z);
        Vector3 newPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref _positionChange, SmoothTime);
        transform.position = StabilizeCameraPosition(newPosition);
    }

    private void ZoomCamera(float targetOrthographicSize, Vector3 offset)
    {
        float smoothTime = 0.38f;
        
        if(_camera.orthographicSize == targetOrthographicSize - 0.1f)
        {
            SetSmoothlyOffset(offset.x, offset.y);
            return;
        }
        
        _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, targetOrthographicSize, ref _zoomVelocity, smoothTime);
        transform.position = Vector3.SmoothDamp(transform.position, offset, ref _positionVelocity, smoothTime);
    }
    

    private Vector3 StabilizeCameraPosition(Vector3 newPosition)
    {
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, newPosition, SmoothTime * Time.fixedDeltaTime);
        smoothedPosition.y = _target.Transform.position.y + _currentoffset.y;

        return smoothedPosition;
    }
}