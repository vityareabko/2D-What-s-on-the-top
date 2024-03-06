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
    public IEnumerator PlayerFallCoroutine();
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
    
    private PlayerConfig playerConfig;
    private PlayerAnimationController _animator;
    private Stamina _stamina;

    private int _jumpDirection;
    
    private bool _isLoseCoroutinePlayerEnternalFalling;
    private bool _isLoseBlockJumpProcess;
    private bool _isStaminaRanOut;
    private bool _isFirstJump = true;
    
    private bool _isSlowdown = false;
    private bool _isPlatform = false;
    private bool _isRoll = false;
    
    public PlayerMover(Stamina stamina, PlayerConfig playerConfig, Player player)
    {
        this.playerConfig = playerConfig;
        _stamina = stamina;
        _player = player;
        _rigidbody = player.GetComponent<Rigidbody2D>();
        _playerTransform = player.transform;
        _transformDetection = player.TransformPlatformDetection;
        
        _plarformLayer = ConstLayer.Platform.ToLayerMask();

        var animator = _player.GetComponent<Animator>(); 
        _animator = new PlayerAnimationController(animator);
        
        EventAggregator.Subscribe<SwitchGameStateToPlayGameEvent>(OnStartGame);
        
        EventAggregator.Subscribe<ClaimRewardEvent>(OnStopCoroiteneLoseFall);
        
    }
    
    public void Dispose()
    {
        EventAggregator.Unsubscribe<SwitchGameStateToPlayGameEvent>(OnStartGame);
        EventAggregator.Subscribe<ClaimRewardEvent>(OnStopCoroiteneLoseFall);
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
        
        if (_isLoseBlockJumpProcess)
            return;

        int jumpDirection = isRightWall ? 1 : -1;
        
        if (isRightWall)
            EventAggregator.Post(this, new SwitchCameraStateOnPlayerRightPlatform());
        else
            EventAggregator.Post(this, new SwitchCameraStateOnPlayerLeftPlatform());


        if (_jumpDirection != jumpDirection && _isPlatform || _isFirstJump)
                _animator.JumpAnimationTrigger();
        
        
        if (_isPlatform || _isFirstJump)
            _rigidbody.velocity = new Vector2(jumpDirection * playerConfig.JumpForce, Mathf.Max(_rigidbody.velocity.y, playerConfig.JumpForce));
        else 
            _rigidbody.velocity = new Vector2(jumpDirection * playerConfig.JumpForce, _rigidbody.velocity.y);
                
        if(_isFirstJump)
            _isFirstJump = false;
        
        
        _stamina.DrainRateStaminaJump(); 

        _jumpDirection = jumpDirection;

        FlipCharacter(jumpDirection);

    }

    public void ProcessSlowDown(bool isSlowDown) => _isSlowdown = isSlowDown;
    
    public void ProcessUpwardRoll()
    {
        if (_isPlatform == false)
            return;
        
        _player.StartCoroutine(RollCoroutine());
        UpwardRoll();
        
        ProcessSlowDown(false); 
    }

    public void LastLosseJump()
    {
        const float LastYJumpVelocity = 3f;
        _rigidbody.gravityScale = 0;
        _isLoseBlockJumpProcess = true;
        _isLoseCoroutinePlayerEnternalFalling = true;
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, LastYJumpVelocity);
        _animator.LoseBounceAnimationTrigger();
        _player.StartCoroutine(SmoothMovementAndRotationToCenter());
    }
    
    private void UpwardRoll()
    {
        _animator.RollUpwardAnimationTrigger();
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, playerConfig.RollVerticalSpeed);
        _stamina.DrainRateStaminaUpwardRoll();
    }

    private void SlowDown()
    {
        _animator.WalkAnimation(playerConfig.WalkVerticalSpeed);
        
        float newVerticalSpeed = Mathf.Max(playerConfig.WalkVerticalSpeed, _rigidbody.velocity.y - playerConfig.DecelerationRate * Time.fixedDeltaTime);
        newVerticalSpeed = Mathf.Max(newVerticalSpeed, playerConfig.WalkVerticalSpeed);
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, newVerticalSpeed);
        
        _stamina.DrainRateStaminaWalking(Time.deltaTime);
        
        ProcessSlowDown(true); 
    }
    
    private void MoveUpward()
    {
        _animator.RunAnimation(playerConfig.RunSpeed);
        
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, playerConfig.RunSpeed);
        
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
        const float characterZRotationOnTheWall = 80f;
        
        var scale = _playerTransform.localScale;
        scale.x = Mathf.Abs(scale.x) * jumpDirection;
        _playerTransform.localScale = scale;
        
        _playerTransform.rotation = Quaternion.Euler(
            _playerTransform.rotation.eulerAngles.x, 
            _playerTransform.rotation.eulerAngles.y, 
            characterZRotationOnTheWall * jumpDirection);
    }
    
    private bool ShouldMoveUpward() => _isPlatform && _isSlowdown == false && _isRoll == false;
    
    private void CheckOnPlatformOnPlatform()
    {
        _isPlatform = Physics2D.OverlapCircle(_transformDetection.position, RadiusDetectionPlatform, _plarformLayer) is not null;
        _animator.IsPlatform(_isPlatform);
    }
    
    private IEnumerator RollCoroutine()
    {
        _isRoll = true;
        UpwardRoll();
        yield return new WaitForSeconds(RollDelay);
        _isRoll = false;
    }
    
    public IEnumerator PlayerFallCoroutine()
    {
        const float initialFallSpeed = -5f;
        const float resetPositionY = 50f;   // # Todo - сейчась я оставлю 50 но нужно будет поменять StartPointPosition + 50f - потому что это будет работать только для первого уровня а дальше уже будет к примеру чекпоинт на y = 500
        const float minimumYPosition = 10f;
        const float maxFallSpeed = -10f;
        const float gravityAcceleration = -9.8f;
        const float finalPositionY = 30f;
        
        var rigidbody = _player.GetComponent<Rigidbody2D>();
        
        rigidbody.velocity = new Vector2(0, initialFallSpeed); 
        
        EventAggregator.Post(this, new PlayerLoseHideAllObstaclesEvent());
        
        while (_isLoseCoroutinePlayerEnternalFalling)
        {
            if (_player.transform.position.y <= minimumYPosition)
            {
                var newPos = _player.transform.position;
                newPos.y = resetPositionY;
                _player.transform.position = newPos;
                rigidbody.velocity = new Vector2(0, initialFallSpeed); 
            }
            else
            {
                if (rigidbody.velocity.y < maxFallSpeed) 
                    rigidbody.velocity = new Vector2(0, maxFallSpeed);
                else
                    rigidbody.velocity += new Vector2(0, Time.deltaTime * gravityAcceleration);
            }
            
            yield return null;
        }
        
        _animator.LoseBouncePrepareToLand(true);
        
        rigidbody.velocity = new Vector2(0, initialFallSpeed); ; 
        var newPosition = _player.transform.position;
        newPosition.y = finalPositionY;
        
        _player.transform.position = newPosition;
        _isLoseBlockJumpProcess = false;
    }

    private IEnumerator SmoothMovementAndRotationToCenter()
    {
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
    
    private void OnStartGame(object sender, SwitchGameStateToPlayGameEvent evendData)
    {
        _isStaminaRanOut = false;
        _stamina.Initialize();
    }
    
    private void OnStopCoroiteneLoseFall(object sender, ClaimRewardEvent eventData) => _isLoseCoroutinePlayerEnternalFalling = false;
}
