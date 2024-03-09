using System;
using System.Collections;
using GameSM;
using GG.Infrastructure.Utils.Swipe;
using MyNamespace.Services.StorageService.SelectorSkin;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour, IPlayer
{
    [field: SerializeField] public Transform TransformPlatformDetection { get; private set; }
    
    public Transform Transform { get; private set; }
    
    private IGameCurrentState _gameCurrentState;

    private SwipeListener _swipeListener;  
    private IPlayerMover _playerMover;

    private PlayerAnimationController _animatorController;
    
    private bool _isBlockSpwipe;
    private bool _isBlockMovement;
    
    [Inject] private void Construct(SwipeListener swipeListener, IGameCurrentState gameCurrentState, IPlayerMover playerMover, ISelectSkin selectSkin)
    {
        _playerMover = playerMover;
        _swipeListener = swipeListener;
        _gameCurrentState = gameCurrentState;
        Transform = transform;
        
        EventAggregator.Post(this, new ApplySelectedHeroSkinEvent() { CurrentSkin = selectSkin.CurrentHeroSkin});
        EventAggregator.Post(this, new ApplySelectedShieldSkinEvent { CurrentShieldSkin = selectSkin.CurrentShieldSkin});
    }
    
    private void Awake() => _animatorController = new PlayerAnimationController(GetComponent<Animator>());
    
    private void OnEnable()
    {
        _swipeListener.OnSwipe.AddListener(OnSwipeHandler);
        EventAggregator.Subscribe<PlayeLoseLastJumpEvent>(OnPlayerLoseLastJumping);
        EventAggregator.Subscribe<GameIsOnPausedEvent>(OnPausedGame);
    }
    
    private void OnDisable()
    {
        _swipeListener.OnSwipe.RemoveListener(OnSwipeHandler);
        EventAggregator.Unsubscribe<PlayeLoseLastJumpEvent>(OnPlayerLoseLastJumping);
        EventAggregator.Unsubscribe<GameIsOnPausedEvent>(OnPausedGame);
    }

    private void Update()
    {
        ObserveGameState();
        
        ResetSlowDownToTouch();
        _playerMover.ProcessCheckingToPlayerAction();
    }
    
    private void FixedUpdate() => _playerMover.ProcessMovement(_isBlockMovement);
    
    private void ObserveGameState()
    {
        var CurrentGameState = _gameCurrentState.CurrentState;

        switch (CurrentGameState)
        {
            case GameStateType.GameMenu:
                // Блокируем все кроме сфайпа - желательно только в лево и вправо
                Debug.Log(GameStateType.GameMenu);
                BlockSwipe(true); 
                BlockMovement(true);
                Invoke(nameof(ResetAnimationBounceLanding), 1f);
                break;
            case GameStateType.GamePlay:
                Debug.Log(GameStateType.GamePlay);
                // unblock all game
                // InitializeStamina();
                BlockSwipe(false);  
                BlockMovement(false);
                break;
            case GameStateType.LoseGame:
                Debug.Log(GameStateType.LoseGame);
                // block player mover and block swipping
                BlockSwipe(true);  
                BlockMovement(true);
                break;
            case GameStateType.WinGame:
                Debug.Log(GameStateType.WinGame);
                // block player mover and block swipping
                BlockSwipe(true);  
                BlockMovement(true);
                break;
            default:
                throw new ArgumentOutOfRangeException($"{CurrentGameState} couldn't found in GameStateType");
        }
    }

    private void ResetAnimationBounceLanding() => _animatorController.LoseBounceLanding(false);

    private void BlockSwipe(bool isBlock) => _isBlockSpwipe = isBlock;
    
    private void BlockMovement(bool isBlock) => _isBlockMovement = isBlock;
    
    private void PlayerLose()
    {
        EventAggregator.Post(this, new SwitchGameStateToLoseGameEvent());
        StartCoroutine(_playerMover.PlayerFallCoroutine());
    }
    
    private void ResetSlowDownToTouch()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            _playerMover.ProcessSlowDown(false); 
    }
    
    private void OnPausedGame(object sender, GameIsOnPausedEvent eventData) => BlockSwipe(eventData.IsOnPause);
    
    private void OnSwipeHandler(string swipe)
    {
        if (_isBlockSpwipe)
            return;
    
        if (swipe == DirectionId.ID_LEFT)
            _playerMover.ProcessJumpinp(false);

        if (swipe == DirectionId.ID_RIGHT)
            _playerMover.ProcessJumpinp(true);

        if (swipe == DirectionId.ID_DOWN)
            _playerMover.ProcessSlowDown(true);
        
        if (swipe == DirectionId.ID_UP)
            _playerMover.ProcessUpwardRoll();
    }
    
    private void OnPlayerLoseLastJumping(object sender, PlayeLoseLastJumpEvent evenDatat) => PlayerLose();
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(ConstTags.Obstacle))
        {
            _playerMover.LastLosseJump();
        }
        
        if (collider.CompareTag(ConstTags.TransitionToMainMenu)) 
            BlockMovement(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(ConstTags.PlatformMainMenu))
        {
            if (_gameCurrentState.CurrentState == GameStateType.LoseGame)
            {
                _animatorController.LoseBounceLanding(true);
                EventAggregator.Post(this, new SwitchGameStateToMainMenuGameEvent());
            }

            if (_gameCurrentState.CurrentState == GameStateType.WinGame)
            {
                EventAggregator.Post(this, new SwitchGameStateToMainMenuGameEvent());
                EventAggregator.Post(this, new SwitchCameraStateOnMainMenuPlatform());
            }

            StartCoroutine(SmothPlayerMoveToCenter());
        }
    }

    private IEnumerator SmothPlayerMoveToCenter()
    {
        float duration = 0.5f;
        float elapsedTime = 0; 
        Vector3 startPosition = transform.position; 
        Vector3 targetPosition = new Vector3(0, startPosition.y, startPosition.z); // Целевая позиция

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            yield return null;
        }

        transform.position = targetPosition;
    }


    #region Gizmoz
        private void OnDrawGizmos() => Gizmos.DrawWireSphere(TransformPlatformDetection.position, 0.2f);
    #endregion
}

