using System;
using GameSM;
using GG.Infrastructure.Utils.Swipe;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour, IPlayer
{
    [field: SerializeField] public Transform TransformPlatformDetection { get; private set; }
    
    public Transform Transform { get; private set; }

    private SwipeListener _swipeListener;  
    private IPlayerMover _playerMover;

    private PlayerAnimationController _animatorController;
    
    // private CharacterData _characterData;
    // private Stamina _stamina;
    

    private IGameCurrentState _gameCurrentState;
    
    private bool _isBlockSpwipe = false;
    private bool _isBlockMovement = false;
    // private bool _isLoseCoroutine;
    
    // private bool _staminaIsInit;
    
    [Inject] private void Construct(SwipeListener swipeListener, IGameCurrentState gameCurrentState, IPlayerMover playerMover)//(Stamina stamina, CharacterData characterData, SwipeListener swipeListener, IGameCurrentState gameCurrentState)
    {
        // _characterData = characterData;
        // _stamina = stamina;
        _playerMover = playerMover;
        _swipeListener = swipeListener;
        _gameCurrentState = gameCurrentState;
        Transform = transform;
    }

    // private void Awake() => _playerMover = new PlayerMover(this, _transformPlatformDetection); //(_stamina, _characterData, this, _transformPlatformDetection);

    private void Awake() => _animatorController = new PlayerAnimationController(GetComponent<Animator>());
    

    private void OnEnable()
    {
        _swipeListener.OnSwipe.AddListener(OnSwipeHandler);

        EventAggregator.Subscribe<PlayeLoseLastJumpEvent>(OnPlayerLoseLastJumping);
        EventAggregator.Subscribe<GameIsOnPausedEvent>(OnPausedGame);
        // EventAggregator.Subscribe<ClaimRewardEvent>(OnStopCoroiteneLoseFall);
    }
    
    private void OnDisable()
    {
        _swipeListener.OnSwipe.RemoveListener(OnSwipeHandler);
        
        EventAggregator.Unsubscribe<PlayeLoseLastJumpEvent>(OnPlayerLoseLastJumping);
        EventAggregator.Unsubscribe<GameIsOnPausedEvent>(OnPausedGame);
        // EventAggregator.Unsubscribe<ClaimRewardEvent>(OnStopCoroiteneLoseFall);
    }
    
    private void Update()
    {
        // if (_lastJump != null)
        //     _lastJump.Tick();
        
        ObserveGameState();
        
        ResetSlowDownToTouch();
        _playerMover.ProcessCheckingToPlayerAction();
    
        // IsOnPlatform = _playerMover.CheckOnPlatformOnPlatform();
    }
    
    private void FixedUpdate() => _playerMover.ProcessMovement(_isBlockMovement);
    
    private void ObserveGameState()
    {
        var CurrentGameState = _gameCurrentState.CurrentState;

        switch (CurrentGameState)
        {
            case GameStateType.GameMenu:
                // Блокируем все кроме сфайпа - желательно только в лево и вправо
                BlockSwipe(true); // блокирует весь свайп (возможно подвинуть _isBlockSwipe - ниже на два условие а имменоо пропустить вайпы влево в право) // левый и правй свайп включаем только когда игра на платформе MainMenuPlatform 
                BlockMovement(true);
                Invoke(nameof(ResetAnimationBounceLanding), 1f);
                break;
            case GameStateType.GamePlay:
                // unblock all game
                // InitializeStamina();
                BlockSwipe(false);  
                BlockMovement(false);
                break;
            case GameStateType.LoseGame:
                // block player mover and block swipping
                BlockSwipe(true);  
                BlockMovement(true);
                break;
            case GameStateType.WinGame:
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
        // _isLoseCoroutine = true;
        // StartCoroutine(PlayerFallCoroutine());
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
        {
            _playerMover.ProcessJumpinp(false);
            // EventAggregator.Post(this, new PlayerJumpedToAgainsWallEvent(){IsRightWall = false});
        }

        if (swipe == DirectionId.ID_RIGHT)
        {
            _playerMover.ProcessJumpinp(true);
            // EventAggregator.Post(this, new PlayerJumpedToAgainsWallEvent() { IsRightWall = true });
        }

        if (swipe == DirectionId.ID_DOWN)
        {
            _playerMover.ProcessSlowDown(true);
        }
    
        if (swipe == DirectionId.ID_UP)
        {
            _playerMover.ProcessUpwardRoll();
        }
    }
    
    private void OnPlayerLoseLastJumping(object sender, PlayeLoseLastJumpEvent evenDatat) => PlayerLose();
    
    // private void OnStopCoroiteneLoseFall(object sender, ClaimRewardEvent eventData) => _isLoseCoroutine = false;
    
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
        }
    }


    #region Gizmoz
        private void OnDrawGizmos() => Gizmos.DrawWireSphere(TransformPlatformDetection.position, 0.2f);
        
    #endregion
}


/// Что я хочу:
/// я хочу чтобы когда игрок проиграл то его подросило на центр и переместил вместе с камерой на y = 50 от стартовой высоты его уровня (если он превышает эту высоту)
/// и чтобы камера следовала за ним и когда он упадет то показывалась Главное Меню
///
/// Что у меня есть :
/// Сейчась у меня ничего нет ничего просто игрок падает вниз под влиянию гравитации (нужно сделать какой-то класс или в текущем что когда инрок падает то его бросает вверх и постепенно он иде к центру)
/// 
/// 
/// Как я могу сделать:
/// мне нужно запомнить стартовую высоту чтобы потом прибавить 50 (если игрок прошел большего ростояния чем startPos + 50) и потом переместить игрока на эту позицию 
///
///
/// 

