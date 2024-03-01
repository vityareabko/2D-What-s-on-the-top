using System;
using System.Collections;
using GameSM;
using GG.Infrastructure.Utils.Swipe;
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
    
    private bool _isBlockSpwipe = false;
    private bool _isBlockMovement = false;
    
    [Inject] private void Construct(SwipeListener swipeListener, IGameCurrentState gameCurrentState, IPlayerMover playerMover)
    {
        _playerMover = playerMover;
        _swipeListener = swipeListener;
        _gameCurrentState = gameCurrentState;
        Transform = transform;
    }
    
    private void Awake() => _animatorController = new PlayerAnimationController(GetComponent<Animator>());
    
    private void OnEnable()
    {
        _swipeListener.OnSwipe.AddListener(OnSwipeHandler);

        // EventAggregator.Subscribe<PlayerEnterInFinishBouncePlaceEvent>(OnPlayerEnterInFinshBouncePlaceHandler);
        EventAggregator.Subscribe<PlayeLoseLastJumpEvent>(OnPlayerLoseLastJumping);
        EventAggregator.Subscribe<GameIsOnPausedEvent>(OnPausedGame);
    }
    
    private void OnDisable()
    {
        _swipeListener.OnSwipe.RemoveListener(OnSwipeHandler);
        
        // EventAggregator.Unsubscribe<PlayerEnterInFinishBouncePlaceEvent>(OnPlayerEnterInFinshBouncePlaceHandler);
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
        float duration = 0.5f; // Длительность перемещения, в секундах
        float elapsedTime = 0; // Время, прошедшее с начала перемещения
        Vector3 startPosition = transform.position; // Начальная позиция
        Vector3 targetPosition = new Vector3(0, startPosition.y, startPosition.z); // Целевая позиция

        while (elapsedTime < duration)
        {
            // Вычисляем прошедшее время с начала перемещения
            elapsedTime += Time.deltaTime;
            // Интерполируем позицию игрока от начальной к целевой
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            yield return null; // Ждем следующего кадра
        }

        // Убедимся, что игрок точно находится в целевой позиции по окончании перемещения
        transform.position = targetPosition;
    }

    // private void OnPlayerEnterInFinshBouncePlaceHandler(object sender, PlayerEnterInFinishBouncePlaceEvent eventData)
    // {
    //     Debug.Log("eptishce");
    //     _isBlockMovement = true;
    //     _isBlockSpwipe = true;
    // }


    #region Gizmoz
        private void OnDrawGizmos() => Gizmos.DrawWireSphere(TransformPlatformDetection.position, 0.2f);
        
    #endregion
}

