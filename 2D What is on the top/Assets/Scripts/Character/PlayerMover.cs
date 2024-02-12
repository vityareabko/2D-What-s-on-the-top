using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviour, IPlayerMover
{
    private const float RadiusOverlap = 0.2f; // вынести можно в константу - но пока пусть сидить
    private const float RollDelay = 0.5f;
    
    public event Action RanOutOfStamin;
    
    [SerializeField] private LayerMask _platformLayer;
    [SerializeField] private Transform _platformDetector;
    
    private Rigidbody2D _rb;

    private CharacterData _characterData;
    private Stamina _stamina;

    private bool _isFacingRight  = true;
    private bool _isDefeat       = false;
        
    private bool _isSlowdown     = false;
    private bool _isPlatform     = false;
    private bool _isRoll         = false;

    [Inject] private void Construct(CharacterData characterData, Stamina stamina)
    {
        _characterData = characterData;
        _stamina = stamina;
    }

    private void Awake() => _rb = GetComponent<Rigidbody2D>(); 

    private void Update()
    {
        TriggerDefeatOnStaminaDepletion();
        CheckOnPlatformOnPlatform();
        ResetSlowDownToTouch();
    }

    private void FixedUpdate()
    {
        ProcessMovement();
    }
    
    private void ProcessMovement()
    {
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

    private void TriggerDefeatOnStaminaDepletion()
    {
        if (_stamina.isEnough() == false && _isDefeat == false)
        {
            _isDefeat = true;
            RanOutOfStamin?.Invoke();
        }
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
    
    private void ResetSlowDownToTouch()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            SlowDownFlag(false); 
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
        _isPlatform = Physics2D.OverlapCircle(_platformDetector.position, RadiusOverlap, _platformLayer) is not null;
    
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
        Gizmos.DrawWireSphere(_platformDetector.position, RadiusOverlap);
    }
        
    #endregion

}

// using System;
// using System.Collections;
// using UI.GameScreenPause;
// using UnityEngine;
// using Zenject;
//
// public class PlayerMover : MonoBehaviour
// {
//     private const float _radiusOverlap = 0.2f; // вынести можно в константу - но пока пусть сидить
//     
//     public event Action<float> DrainStaminaRunningWalking;
//     public event Action<float> DrainStaminaForAction;
//     
//     [SerializeField] private LayerMask _platformLayer;
//     [SerializeField] private Rigidbody2D _rb;
//     [SerializeField] private Transform _overlapChecker;
//
//     private CharacterData _characterData;
//     
//     private bool _hasEnoughStamina = true;
//     private bool _isFacingRight = true;
//     private bool _isSlowdown;
//     private bool _isPlatform;
//     private bool _isRoll;
//     
//     [Inject] private void Construct(CharacterData characterData) => _characterData = characterData;
//     
//     private void Update()
//     {
//         CheckOnPlatformOnPlatform();
//         ResetSlowDownToTouch();
//     }
//
//     private void FixedUpdate()
//     {
//         if (_hasEnoughStamina == false)
//             return;
//         
//         if (_isPlatform && _isSlowdown == false  && _isRoll == false)
//             MoveUpward();
//
//         if (_isSlowdown)
//             SlowDown();
//     }
//
//     public void StaminaIsEnough(bool isEnough) => _hasEnoughStamina = isEnough;
//     
//     public void Jump(bool isRightWall)
//     {
//         if (_isPlatform == false) // TODO: - это можно убрать чтобы сделать как фичу - что можно прыгать как хочешь
//             return;
//
//         if (_isFacingRight == isRightWall)
//             return;
//
//         _isFacingRight = !_isFacingRight;
//         float jumpDirection = _isFacingRight ? 1 : -1;
//
//         _rb.velocity = new Vector2(jumpDirection * _characterData.JumpForce, _rb.velocity.y);
//         DrainStaminaForAction?.Invoke(_characterData.StaminaData.StaminaDrainRateJumping);
//         
//         FlipCharacter();
//     }
//     
//     public void SlowDownFlag(bool isSlowDown) => _isSlowdown = isSlowDown;
//     
//     public void PerformRoll()
//     {
//         StartCoroutine(RollCoroutine());
//         SlowDownFlag(false); 
//     }
//     
//     private void UpwardRoll()
//     {
//         _rb.velocity = new Vector2(_rb.velocity.x, _characterData.MaxVerticalSpeed);
//         DrainStaminaForAction?.Invoke(_characterData.StaminaData.StaminaDrainRateRoll);
//     }
//
//     private void SlowDown()
//     {
//         float newVerticalSpeed = Mathf.Max(_characterData.MinVerticalSpeed, _rb.velocity.y - _characterData.DecelerationRate * Time.fixedDeltaTime);
//         newVerticalSpeed = Mathf.Max(newVerticalSpeed, _characterData.MinVerticalSpeed);
//         _rb.velocity = new Vector2(_rb.velocity.x, newVerticalSpeed);
//         
//         DrainStaminaRunningWalking?.Invoke(_characterData.StaminaData.StaminaDrainRateWalking * Time.deltaTime);
//         
//         SlowDownFlag(true); 
//     }
//     
//     private void MoveUpward()
//     {
//         _rb.velocity = new Vector2(_rb.velocity.x, _characterData.DefaultSpeed);
//         
//         DrainStaminaRunningWalking?.Invoke(_characterData.StaminaData.StaminaDrainRateRunning * Time.deltaTime);
//         
//         SlowDownFlag(false); 
//     }
//     
//     private void ResetSlowDownToTouch()
//     {
//         if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
//             SlowDownFlag(false); 
//     }
//     
//     private void FlipCharacter()
//     {
//         var scale = transform.localScale;
//         scale.x *= -1;
//         transform.localScale = scale;
//         
//         var transformRotation = transform.rotation;
//         transformRotation.z *= -1;
//         transform.rotation = transformRotation;
//     }
//     
//     private IEnumerator RollCoroutine()
//     {
//         _isRoll = true;
//         UpwardRoll();
//         yield return new WaitForSeconds(0.5f);
//         _isRoll = false;
//     }
//     
//     private void CheckOnPlatformOnPlatform() => 
//         _isPlatform = Physics2D.OverlapCircle(_overlapChecker.position, _radiusOverlap, _platformLayer) is not null;
//     
//     #region Events
//
//     private void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (((1 << collision.gameObject.layer) & _platformLayer) != 0)
//             MoveUpward();
//     }
//
//     #endregion
//     
//     #region Gizmoz
//     
//     private void OnDrawGizmos()
//     {
//         Gizmos.DrawWireSphere(_overlapChecker.position, _radiusOverlap);
//     }
//         
//     #endregion
// }