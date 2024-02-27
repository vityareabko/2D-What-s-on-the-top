using System.Collections;
using Extensions;
using Helper;
using UnityEngine;

public class PlayerMover 
{
    private const float RadiusDetectionPlatform = 0.2f;
    private const float RollDelay = 0.5f;
    
    private MonoBehaviour _behaviour;
    private Transform _playerTransform;
    private Transform _transformDetection;
    private LayerMask _plarformLayer;
    private Rigidbody2D _rb;
    
    private CharacterData _characterData;
    private PlayerAnimationController _animator;
    private Stamina _stamina;

    private int _jumpDirection;

    private bool _isSlowdown = false;
    private bool _isPlatform = false;
    private bool _isRoll = false;
    
    public PlayerMover(Stamina stamina, CharacterData characterData, MonoBehaviour behaviour, Transform transformPlatformDetection)
    {
        _characterData = characterData;
        _stamina = stamina;
        _behaviour = behaviour;
        _rb = _behaviour.GetComponent<Rigidbody2D>();
        _playerTransform = _behaviour.transform;
        _transformDetection = transformPlatformDetection;
        _plarformLayer = ConstLayer.Platform.ToLayerMask();
        
        var animator = _behaviour.GetComponent<Animator>();
        _animator = new PlayerAnimationController(animator);
    }
    
    public void ProcessMovement(bool isBlockMovemnt)
    {
        if (isBlockMovemnt)
        {
            _animator.IdleAnimation();
            return;
        }
        
        if (ShouldMoveUpward())
        {
            MoveUpward();
            return; 
        }
        
        if (_isSlowdown && _isPlatform)
        {
            SlowDown();
        }
    }

    public void ProcessCheckingToPlayerAction()
    {
        CheckStaminaDepletion();
        CheckOnPlatformOnPlatform();
    }

    public bool CheckOnPlatformOnPlatform()
    {
        _isPlatform = Physics2D.OverlapCircle(_transformDetection.position, RadiusDetectionPlatform, _plarformLayer) is not null;
        _animator.IsPlatform(_isPlatform);
        return _isPlatform;
    }
    
    public void Jump(bool isRightWall)
    {
        // if (_isPlatform == false) // TODO: - это можно убрать чтобы сделать как фичу - что можно прыгать как хочешь
        //     return;
        
        int jumpDirection = isRightWall ? 1 : -1;
        
        if (_jumpDirection == null | _jumpDirection != jumpDirection)
            if (_isPlatform) 
                _animator.JumpAnimation();
        

        _rb.velocity = new Vector2(jumpDirection * _characterData.JumpForce, Mathf.Max(_rb.velocity.y, _characterData.JumpForce));
        _stamina.DrainRateStaminaJump();

        _jumpDirection = jumpDirection;
        
        FlipCharacter(jumpDirection);
        
    }
    
    public void SlowDownFlag(bool isSlowDown) => _isSlowdown = isSlowDown;
    
    public void PerformRoll()
    {
        _behaviour.StartCoroutine(RollCoroutine());
        SlowDownFlag(false); 
    }
    
    private void UpwardRoll()
    {
        _animator.RollUpwardAnimation();
        
        _rb.velocity = new Vector2(_rb.velocity.x, _characterData.RollVerticalSpeed);
        _stamina.DrainRateStaminaUpwardRoll();
    }

    private void SlowDown()
    {
        _animator.WalkAnimation(_characterData.WalkVerticalSpeed);
        
        float newVerticalSpeed = Mathf.Max(_characterData.WalkVerticalSpeed, _rb.velocity.y - _characterData.DecelerationRate * Time.fixedDeltaTime);
        newVerticalSpeed = Mathf.Max(newVerticalSpeed, _characterData.WalkVerticalSpeed);
        _rb.velocity = new Vector2(_rb.velocity.x, newVerticalSpeed);
        
        _stamina.DrainRateStaminaWalking(Time.deltaTime);
        
        SlowDownFlag(true); 
    }
    
    private void MoveUpward()
    {
        _animator.RunAnimation(_characterData.RunSpeed);
        
        _rb.velocity = new Vector2(_rb.velocity.x, _characterData.RunSpeed);
        
        _stamina.DrainRateStaminaRun(Time.deltaTime);
        
        SlowDownFlag(false); 
    }
    
    private void CheckStaminaDepletion()
    {
        if (_stamina.isEnough() == false )
        {
            EventAggregator.Post(this, new PlayeRanOutOfStaminaEventHandler());
        }
    }

    private void FlipCharacter(float jumpDirection)
    {

        var scale = _playerTransform.localScale;
        scale.x = Mathf.Abs(scale.x) * jumpDirection;
        _playerTransform.localScale = scale;
        
        _playerTransform.rotation = Quaternion.Euler(  // # todo - можно оставить потому что прикольно что персонаж иногода перемещается задом наперед но нужно что-то придумать с анимацией вогда персонаж пригает вперед 
            _playerTransform.rotation.eulerAngles.x, 
            _playerTransform.rotation.eulerAngles.y, 
            80f * jumpDirection);
    }
    
    private bool ShouldMoveUpward() => _isPlatform && _isSlowdown == false && _isRoll == false;
    
    private IEnumerator RollCoroutine()
    {
        _isRoll = true;
        UpwardRoll();
        yield return new WaitForSeconds(RollDelay);
        _isRoll = false;
    }
    
}
