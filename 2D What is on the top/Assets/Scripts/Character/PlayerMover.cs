using System;
using System.Collections;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviour, IPlayerMover
{
    // playerMover можна вынести в обычный не монобех класс
    
    private const float RadiusDetectionPlatform = 0.2f;
    private const float RollDelay               = 0.5f;
    
    public event Action RanOutOfStamin;
    
    [SerializeField] private LayerMask _platformLayer;
    [SerializeField] private Transform _platformDetector;
    
    private Rigidbody2D _rb;

    private CharacterData _characterData;
    private Stamina _stamina;

    private bool _isFacingRight    = true;
    private bool _isDefeat         = false;
          
    private bool _isSlowdown       = false;
    private bool _isPlatform       = false;
    private bool _isRoll           = false;

    [Inject] private void Construct(CharacterData characterData, Stamina stamina)
    {
        _characterData = characterData;
        _stamina = stamina;
    }

    private void Awake() => _rb = GetComponent<Rigidbody2D>(); 
    
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
        
        if (_isSlowdown)
        {
            SlowDown();
        }
    }

    public void ProcessCheckingToPlayerAction()
    {
        TriggerDefeatOnStaminaDepletion();
        CheckOnPlatformOnPlatform();
        // ResetSlowDownToTouch();
    }
    
    public void FreezePlayer(bool _isFreeze)
    {
        if (_isFreeze)
            _rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        else
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    
    public void Jump(bool isRightWall)
    {
        if (_isPlatform == false) // TODO: - это можно убрать чтобы сделать как фичу - что можно прыгать как хочешь
            return;

        if (_isFacingRight == isRightWall)
            return;

        _isFacingRight = !_isFacingRight;
        float jumpDirection = _isFacingRight ? 1 : -1;

        _rb.velocity = new Vector2(jumpDirection * _characterData.JumpForce, _rb.velocity.y);
        _stamina.DrainStaminaJump();
        
        FlipCharacter();
    }
    
    public void SlowDownFlag(bool isSlowDown) => _isSlowdown = isSlowDown;
    
    public void PerformRoll()
    {
        StartCoroutine(RollCoroutine());
        SlowDownFlag(false); 
    }
    
    private void UpwardRoll()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, _characterData.MaxVerticalSpeed);
        _stamina.DrainStaminaUpwardRoll();
    }

    private void SlowDown()
    {
        float newVerticalSpeed = Mathf.Max(_characterData.MinVerticalSpeed, _rb.velocity.y - _characterData.DecelerationRate * Time.fixedDeltaTime);
        newVerticalSpeed = Mathf.Max(newVerticalSpeed, _characterData.MinVerticalSpeed);
        _rb.velocity = new Vector2(_rb.velocity.x, newVerticalSpeed);
        
        _stamina.DrainStaminaWalking(Time.deltaTime);
        
        SlowDownFlag(true); 
    }
    
    private void MoveUpward()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, _characterData.DefaultSpeed);
        
        _stamina.DrainStaminaRun(Time.deltaTime);
        
        SlowDownFlag(false); 
    }
    
    private void TriggerDefeatOnStaminaDepletion()
    {
        if (_stamina.isEnough() == false && _isDefeat == false)
        {
            _isDefeat = true;
            RanOutOfStamin?.Invoke();
        }
    }
    
    private void FlipCharacter()
    {
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        
        var transformRotation = transform.rotation;
        transformRotation.z *= -1;
        transform.rotation = transformRotation;
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
        _isPlatform = Physics2D.OverlapCircle(_platformDetector.position, RadiusDetectionPlatform, _platformLayer) is not null;
    
    #region Events

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & _platformLayer) != 0)
            MoveUpward();
    }

    #endregion
    
    #region Gizmoz
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_platformDetector.position, RadiusDetectionPlatform);
    }
        
    #endregion

}
