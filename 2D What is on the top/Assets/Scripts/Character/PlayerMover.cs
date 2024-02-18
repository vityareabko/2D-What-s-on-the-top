using System.Collections;
using Extensions;
using Helper;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover 
{
    private const float RadiusDetectionPlatform = 0.2f;
    private const float RollDelay = 0.5f;
    
    private MonoBehaviour _behaviour;
    private Transform _playerTransform;
    private Transform _transformDetection;
    private LayerMask _plarformLayer;
    private Animator _animator;
    private Rigidbody2D _rb;
    
    private CharacterData _characterData;
    private Stamina _stamina;

    private bool _isFacingRight = true;
    private bool _isDefeat = false;

    private bool _isSlowdown = false;
    private bool _isPlatform = false;
    private bool _isRoll = false;
    
    public PlayerMover(Stamina stamina, CharacterData characterData, MonoBehaviour behaviour, Transform transformPlatformDetection, Animator animator)
    {
        _characterData = characterData;
        _stamina = stamina;
        _behaviour = behaviour;
        _rb = _behaviour.GetComponent<Rigidbody2D>();
        _playerTransform = _behaviour.transform;
        _animator = animator;
        _transformDetection = transformPlatformDetection;
        _plarformLayer = ConstLayer.Platform.ToLayerMask();
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
        _animator.SetBool("IsPlatform", _isPlatform);
        
    }
    
    // public void FreezePlayer(bool _isFreeze)
    // {
    //     if (_isFreeze)
    //         _rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    //     else
    //         _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    // }
    
    public void Jump(bool isRightWall)
    {
        // if (_isPlatform == false) // TODO: - это можно убрать чтобы сделать как фичу - что можно прыгать как хочешь
        //     return;
        
        if (_isFacingRight == isRightWall)
            return;
        
        if (_isPlatform)
            _animator.SetTrigger("Jump");
        
        _isFacingRight = !_isFacingRight;
        float jumpDirection = _isFacingRight ? 1 : -1;

        _rb.velocity = new Vector2(jumpDirection * _characterData.JumpForce, _rb.velocity.y);
        _stamina.DrainRateStaminaJump();
        
        FlipCharacter();
    }
    
    public void SlowDownFlag(bool isSlowDown) => _isSlowdown = isSlowDown;
    
    public void PerformRoll()
    {
        _behaviour.StartCoroutine(RollCoroutine());
        SlowDownFlag(false); 
    }
    
    private void UpwardRoll()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, _characterData.MaxVerticalSpeed);
        _stamina.DrainRateStaminaUpwardRoll();
    }

    private void SlowDown()
    {
        float newVerticalSpeed = Mathf.Max(_characterData.MinVerticalSpeed, _rb.velocity.y - _characterData.DecelerationRate * Time.fixedDeltaTime);
        newVerticalSpeed = Mathf.Max(newVerticalSpeed, _characterData.MinVerticalSpeed);
        _rb.velocity = new Vector2(_rb.velocity.x, newVerticalSpeed);
        
        _stamina.DrainRateStaminaWalking(Time.deltaTime);
        
        SlowDownFlag(true); 
    }
    
    private void MoveUpward()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, _characterData.DefaultSpeed);
        
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
    
    private void CheckOnPlatformOnPlatform() => 
        _isPlatform = Physics2D.OverlapCircle(_transformDetection.position, RadiusDetectionPlatform, _plarformLayer) is not null;

}
