using System;
using System.Collections;
using Extensions;
using Helper;
using UnityEngine;

public interface IPlayerMover
{
    public void ProcessCheckingToPlayerAction();
    public void ProcessMovement(bool isBlockMovemnt);
    public void ProcessSlowDown(bool isSlowDown);
    public void ProcessUpwardRoll();
    public void ProcessJumpinp(bool isRightWall);
    public void LastLosseJump();
}

public class PlayerMover : IPlayerMover, IDisposable
{
    private const float RadiusDetectionPlatform = 0.2f;
    private const float RollDelay = 0.5f;
    
    private Player _player;
    private Transform _playerTransform;
    private Transform _transformDetection;
    private LayerMask _plarformLayer;
    private Rigidbody2D _rigidbody;
    
    private CharacterData _characterData;
    private PlayerAnimationController _animator;
    private Stamina _stamina;

    private int _jumpDirection;
    private bool _isStaminaRanOut;
    
    private bool _isSlowdown = false;
    private bool _isPlatform = false;
    private bool _isRoll = false;
    
    public PlayerMover(Stamina stamina, CharacterData characterData, Player player)
    {
        _characterData = characterData;
        _stamina = stamina;
        _player = player;
        _rigidbody = player.GetComponent<Rigidbody2D>();
        _playerTransform = player.transform;
        _transformDetection = player.TransformPlatformDetection;
        
        _plarformLayer = ConstLayer.Platform.ToLayerMask();
        
        var animator = this._player.GetComponent<Animator>();
        _animator = new PlayerAnimationController(animator);
        
        EventAggregator.Subscribe<SwitchGameStateToPlayGameEvent>(OnStartGame);
        
    }
    
    public void Dispose() => EventAggregator.Unsubscribe<SwitchGameStateToPlayGameEvent>(OnStartGame);

    private void OnStartGame(object sender, SwitchGameStateToPlayGameEvent evendData)
    {
        _isStaminaRanOut = false;
        _stamina.Initialize();
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

    public void ProcessJumpinp(bool isRightWall)
    {
        // if (_isPlatform == false) // TODO: - это можно убрать чтобы сделать как фичу - что можно прыгать как хочешь
        //     return;

        int jumpDirection = isRightWall ? 1 : -1;
        
        if (isRightWall)
            EventAggregator.Post(this, new SwitchCameraStateOnPlayerRightPlatform());
        else
            EventAggregator.Post(this, new SwitchCameraStateOnPlayerLeftPlatform());
        
        
        if (_jumpDirection == null | _jumpDirection != jumpDirection)
            if (_isPlatform) // это для далнейшых прыжков 
                _animator.JumpAnimation();


        // if (_isPlatform) // это нужно чтобы игрок изначально был на платформе (если буде реализовувать чтобы игра началась при свайпе или при нажатия кнопки но чтобы игрок прыгнул на стену и потом уже включать эту проверку)
        // {
            _rigidbody.velocity = new Vector2(jumpDirection * _characterData.JumpForce, Mathf.Max(_rigidbody.velocity.y, _characterData.JumpForce));
            _stamina.DrainRateStaminaJump();
        // }



        _jumpDirection = jumpDirection;

        FlipCharacter(jumpDirection);

    }

    public void ProcessSlowDown(bool isSlowDown) => _isSlowdown = isSlowDown;
    
    public void ProcessUpwardRoll()
    {
        if (_isPlatform == false)
            return;
        
        _player.StartCoroutine(RollCoroutine());
        ProcessSlowDown(false); 
    }

    public void LastLosseJump()
    {
        _rigidbody.gravityScale = 0;
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 15f);
        _player.StartCoroutine(SmoothMovementAndRotationToCenter());
    }
    
    private void UpwardRoll()
    {
        _animator.RollUpwardAnimation();
        
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _characterData.RollVerticalSpeed);
        _stamina.DrainRateStaminaUpwardRoll();
    }

    private void SlowDown()
    {
        _animator.WalkAnimation(_characterData.WalkVerticalSpeed);
        
        float newVerticalSpeed = Mathf.Max(_characterData.WalkVerticalSpeed, _rigidbody.velocity.y - _characterData.DecelerationRate * Time.fixedDeltaTime);
        newVerticalSpeed = Mathf.Max(newVerticalSpeed, _characterData.WalkVerticalSpeed);
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, newVerticalSpeed);
        
        _stamina.DrainRateStaminaWalking(Time.deltaTime);
        
        ProcessSlowDown(true); 
    }
    
    private void MoveUpward()
    {
        _animator.RunAnimation(_characterData.RunSpeed);
        
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _characterData.RunSpeed);
        
        _stamina.DrainRateStaminaRun(Time.deltaTime);
        
        ProcessSlowDown(false); 
    }
    
    private void CheckStaminaDepletion()
    {
        if (_isStaminaRanOut)
            return;
        
        if (_stamina.isEnough() == false )
        {
            _isStaminaRanOut = true;
            LastLosseJump();
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
    
    private void CheckOnPlatformOnPlatform()
    {
        _isPlatform = Physics2D.OverlapCircle(_transformDetection.position, RadiusDetectionPlatform, _plarformLayer) is not null;
        _animator.IsPlatform(_isPlatform);
    }
    
    private bool ShouldMoveUpward() => _isPlatform && _isSlowdown == false && _isRoll == false;
    
    private IEnumerator RollCoroutine()
    {
        _isRoll = true;
        UpwardRoll();
        yield return new WaitForSeconds(RollDelay);
        _isRoll = false;
    }

    private IEnumerator SmoothMovementAndRotationToCenter()
    {
        // Пока текущая позиция игрока по оси X не приблизится к 0 или пока поворот не станет равным 0
        while (Mathf.Abs(_playerTransform.position.x) > 0.01f || Mathf.Abs(_playerTransform.eulerAngles.z) > 0.01f)
        {
            // Плавно изменяем позицию игрока к центру
            _playerTransform.position = Vector3.MoveTowards(_playerTransform.position, new Vector3(0, _playerTransform.position.y, _playerTransform.position.z), Time.deltaTime * 2f);
        
            // Плавно корректируем поворот до z = 0
            Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
            _playerTransform.rotation = Quaternion.RotateTowards(_playerTransform.rotation, targetRotation, Time.deltaTime * 100f);
        
            yield return null; // Ожидаем следующий кадр
        }
    
        // Включаем гравитацию обратно
        _rigidbody.gravityScale = 1;
        EventAggregator.Post(this, new PlayeLoseLastJumpEvent());
            
    }
}
