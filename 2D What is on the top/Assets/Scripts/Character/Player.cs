using System;

using GameSM;
using GG.Infrastructure.Utils.Swipe;
using Triggers;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour, IPlayer
{
    [SerializeField] private Transform _transformPlatformDetection;
    
    public Transform Transform { get; private set; }
    public bool IsOnPlatform { get; private set; } // это для камеры
    
    private SwipeListener _swipeListener;  
    private CharacterData _characterData;
    private PlayerMover _playerMover;
    private Stamina _stamina;
    

    private IGameCurrentState _gameCurrentState;
    
    private bool _isBlockSpwipe = false;
    private bool _isBlockMovement = false;
    
    [Inject] private void Construct(Stamina stamina, CharacterData characterData, SwipeListener swipeListener, IGameCurrentState gameCurrentState)
    {
        _characterData = characterData;
        _swipeListener = swipeListener;
        _stamina = stamina;
        _gameCurrentState = gameCurrentState;
        Transform = transform;
    }
    
    private void Awake() => _playerMover = new PlayerMover(_stamina, _characterData, this, _transformPlatformDetection);
    
    private void OnEnable()
    {
        _swipeListener.OnSwipe.AddListener(OnSwipeHandler);
        EventAggregator.Subscribe<PlayeRanOutOfStaminaEventHandler>(OnRanOutOfStamin);
        EventAggregator.Subscribe<GameIsOnPausedEvent>(OnPausedGame);
    }
    
    private void OnDisable()
    {
        _swipeListener.OnSwipe.RemoveListener(OnSwipeHandler);
        EventAggregator.Unsubscribe<PlayeRanOutOfStaminaEventHandler>(OnRanOutOfStamin);
        EventAggregator.Unsubscribe<GameIsOnPausedEvent>(OnPausedGame);
    }
    
    private void Update()
    {
        if (_lastJump != null)
            _lastJump.Tick();
        
        ObserveGameState();
        
        ResetSlowDownToTouch();
        _playerMover.ProcessCheckingToPlayerAction();
    
        IsOnPlatform = _playerMover.CheckOnPlatformOnPlatform();
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
                break;
            case GameStateType.GamePlay:
                // unblock all game
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
    
  
    private void BlockSwipe(bool isBlock) => _isBlockSpwipe = isBlock;
    
    private void BlockMovement(bool isBlock) => _isBlockMovement = isBlock;
    
    private void PlayerLose()
    {
        EventAggregator.Post(this, new SwitchGameStateToLoseGameEvent());
        
        // EventAggregator.Post(this, new SwitchGameStateEvent(){ CurrentGameState = GameStateType.LoseGame });
        // EventAggregator.Post(this, new PlayerLoseEventHandler()); - // НУЖНО ПРОВЕРИТЬ ПОТОМ - ПОТОМУ ЧТО НА ЭТОМ ЕВЕНТЕ МНОГОЕ ВЕСИТ
    }
    
    private void ResetSlowDownToTouch()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            _playerMover.SlowDownFlag(false); 
    }

   
    
    private LastJumpToCenter _lastJump; // # todo - нужно будет что-то придумать по лучше а то это сдесь как-то не очень (но это не кретично можно оставить на последок)
    // # todo - мне не нравится сдесь то что мне нужно переключать Player.enable = true / false - из-за этого может поломатся что-то например как ссейчас камера
    private void LastJump()
    {
        _lastJump = new LastJumpToCenter(this, 4f, 1f);
        _lastJump.StartLastJump();
    } 

    
    
    
    private void OnPausedGame(object sender, GameIsOnPausedEvent eventData) => BlockSwipe(eventData.IsOnPause);
    
    private void OnSwipeHandler(string swipe)
    {
        if (_isBlockSpwipe)
            return;
    
        if (swipe == DirectionId.ID_LEFT)
        {
            _playerMover.Jump(false);
            EventAggregator.Post(this, new PlayerJumpedToAgainsWallEvent(){IsRightWall = false});
        }

        if (swipe == DirectionId.ID_RIGHT)
        {
            _playerMover.Jump(true);
            EventAggregator.Post(this, new PlayerJumpedToAgainsWallEvent() { IsRightWall = true });
        }

        if (swipe == DirectionId.ID_DOWN)
        {
            _playerMover.SlowDownFlag(true);
        }
    
        if (swipe == DirectionId.ID_UP)
        {
            _playerMover.PerformRoll();
        }
    }
    
    private void OnRanOutOfStamin(object sender, PlayeRanOutOfStaminaEventHandler eventHandler) =>
        PlayerLose();
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(ConstTags.Obstacle))
        {
            LastJump();
            PlayerLose();
        }

        if (collider.CompareTag(ConstTags.TransitionToMainMenu)) 
            BlockMovement(true);
    }
    
    
    #region Gizmoz
        private void OnDrawGizmos() => Gizmos.DrawWireSphere(_transformPlatformDetection.position, 0.2f);
        
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

