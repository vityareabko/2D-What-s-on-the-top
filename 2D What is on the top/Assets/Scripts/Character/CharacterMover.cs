
using System;
using System.Collections;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    private const float _radiusOverlap = 0.2f; // вынести можно в константу - но пока пусть сидить

    [SerializeField] private LayerMask _platformLayer;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Transform _overlapChecker;

    private CharacterData _characterData;
    private Stamina _stamina;
    
    private bool isFacingRight = true;
    private bool IsSlowdown;
    private bool isPlatform;
    private bool isRoll;

    public void Initialize(CharacterData characterData ,Stamina stamina)
    {
        _characterData = characterData;
        _stamina = stamina;
    }

    private void Update()
    {
        CheckOnPlatformOnPlatform();
        ResetSlowDownToTouch();
    }

    private void FixedUpdate()
    {
        if (_stamina.isEnough() == false)
            return;
        
        if (isPlatform && IsSlowdown == false  && isRoll == false)
            MoveUpward();

        if (IsSlowdown)
            SlowDown();
    }
    
    public void Jump(bool isRightWall)
    {
        if (isPlatform == false) // TODO: - это можно убрать чтобы сделать как фичу - что можно прыгать как хочешь
            return;

        if (isRightWall == isFacingRight)
            return;

        isFacingRight = !isFacingRight;
        float jumpDirection = isFacingRight ? 1 : -1;

        _rb.velocity = new Vector2(jumpDirection * _characterData.JumpForce, _rb.velocity.y);
        
        _stamina.DrainStamina(_characterData.StaminaDrainRateJumping);
        
        FlipCharacter();
    }
    
    public void SlowDownFlag(bool isSlowDown) => IsSlowdown = isSlowDown;
    
    public void PerformRoll()
    {
        StartCoroutine(RollCoroutine());
        _stamina.DrainStamina(_characterData.StaminaDrainRateRoll);
        SlowDownFlag(false); 
    }
    
    private void ResetSlowDownToTouch()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            SlowDownFlag(false); 
    }

    private void SlowDown()
    {
        float newVerticalSpeed = Mathf.Max(_characterData.MinVerticalSpeed, _rb.velocity.y - _characterData.DecelerationRate * Time.fixedDeltaTime);
        newVerticalSpeed = Mathf.Max(newVerticalSpeed, _characterData.MinVerticalSpeed);
        _rb.velocity = new Vector2(_rb.velocity.x, newVerticalSpeed);
        
        _stamina.DrainStamina(_characterData.StaminaDrainRateWalking * Time.deltaTime);
        
        SlowDownFlag(true); 
    }
    
    private void MoveUpward()
    {
        if (_stamina is null)
            return;
        
        _rb.velocity = new Vector2(_rb.velocity.x, _characterData.DefaultSpeed);
        _stamina.DrainStamina(_characterData.StaminaDrainRateRunning * Time.deltaTime);
        
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
    
    
    private IEnumerator RollCoroutine()
    {
       
        isRoll = true;
        _rb.velocity = new Vector2(_rb.velocity.x, _characterData.MaxVerticalSpeed);
        
        yield return new WaitForSeconds(0.5f);
        
        _rb.velocity = new Vector2(_rb.velocity.x, _characterData.DefaultSpeed);
        isRoll = false;
    }
    
    private void CheckOnPlatformOnPlatform() => isPlatform = Physics2D.OverlapCircle(_overlapChecker.position, _radiusOverlap, _platformLayer) is not null;
    
    #region Events

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & _platformLayer) != 0)
        {
            // MoveUpward() вместо того что ниже - попробовать
            _rb.velocity = new Vector2(transform.position.x, _characterData.DefaultSpeed);
        }
    }

    #endregion
    
    #region Gizmoz
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_overlapChecker.position, _radiusOverlap);
    }
        
    #endregion
}