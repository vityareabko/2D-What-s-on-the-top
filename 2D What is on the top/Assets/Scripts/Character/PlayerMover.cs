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

    private bool _isFacingRight = true;
    private bool _isDefeat = false;

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
        if (isBlockMovemnt) return;
        
        if (_isDefeat)
            return;
        
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
    
    public void Jump(bool isRightWall)
    {
        // if (_isPlatform == false) // TODO: - это можно убрать чтобы сделать как фичу - что можно прыгать как хочешь
        //     return;
        
        if (_isFacingRight == isRightWall)
            return;
        
        _animator.JumpAnimation();
    
        // Увеличиваем силу прыжка на основе горизонтальной скорости
        // float extraJumpForce = Mathf.Clamp(Mathf.Abs(_rb.velocity.x), 0, _characterData.JumpForce);
        // float jumpForce = _characterData.JumpForce + extraJumpForce;

        _isFacingRight = !_isFacingRight;
        float jumpDirection = _isFacingRight ? 1 : -1;

        // Применяем горизонтальный и вертикальный импульс
        _rb.velocity = new Vector2(jumpDirection * _characterData.JumpForce, Mathf.Max(_rb.velocity.y, _characterData.JumpForce));
        _stamina.DrainRateStaminaJump();
    
        FlipCharacter();
        
        // if (_isFacingRight == isRightWall)
        //     return;
        //
        // if (_isPlatform)
        //     _animator.JumpAnimation();
        //
        // _isFacingRight = !_isFacingRight;
        // float jumpDirection = _isFacingRight ? 1 : -1;
        //
        // _rb.velocity = new Vector2(jumpDirection * _characterData.JumpForce, _rb.velocity.y);
        // _stamina.DrainRateStaminaJump();
        //
        // FlipCharacter();
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
        if (_stamina.isEnough() == false && _isDefeat == false)
        {
            _isDefeat = true;
            EventAggregator.Post(this, new PlayeRanOutOfStaminaEventHandler());
        }
    }
    
    private void CheckOnPlatformOnPlatform()
    {
        _isPlatform = Physics2D.OverlapCircle(_transformDetection.position, RadiusDetectionPlatform, _plarformLayer) is not null;
        _animator.IsPlatform(_isPlatform);
    }

    private void FlipCharacter()
    {
        var scale = _playerTransform.localScale;
        scale.x *= -1;
        _playerTransform.localScale = scale;
        
        var transformRotation = _playerTransform.rotation;
        transformRotation.z *= -1;
        _playerTransform.rotation = transformRotation;
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
