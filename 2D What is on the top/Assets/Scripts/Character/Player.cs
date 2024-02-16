using System.Collections;
using GG.Infrastructure.Utils.Swipe;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    private const float DefeatDelay = 0.7f;
    private const float CharacterHideDelay = 2f;
    
    [SerializeField] private Transform _transformPlatformDetection;

    private SwipeListener _swipeListener;  
    private PlayerMover _playerMover;

    private Stamina _stamina;
    private CharacterData _characterData;
    
    private bool _isBlockSpwipe = false;
    private bool _isBlockMovement = false;
    private bool _isStopPlayer = false;

    [Inject] private void Construct(Stamina stamina, CharacterData characterData, SwipeListener swipeListener)
    {
        _characterData = characterData;
        _swipeListener = swipeListener;
        _stamina = stamina;
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
        if (_isStopPlayer)
            return;
        
        ResetSlowDownToTouch();
        _playerMover.ProcessCheckingToPlayerAction();
    }

    private void FixedUpdate()
    {
        if (_isStopPlayer)
            return;
        
        _playerMover.ProcessMovement(_isBlockMovement);
    }

    public void BlockSwipe(bool isBlock) => _isBlockSpwipe = isBlock;
    
    private void PlayerWin()
    {
        _isBlockSpwipe = true;
        _isBlockMovement = true;
        _isStopPlayer = true;
        EventAggregator.Post(this, new PlayerWinEventHandler());
    }
    
    private void PlayerLose()
    {
        _isBlockSpwipe = true;
        _isBlockMovement = true;
        _isStopPlayer = true;
        EventAggregator.Post(this, new PlayerLoseEventHandler());
    }


    private void ResetSlowDownToTouch()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            _playerMover.SlowDownFlag(false); 
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
            EventAggregator.Post(this, new PlayerJumpedToAgainsWallEvent(){IsRightWall = true});
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
    
    private void OnRanOutOfStamin(object sender, PlayeRanOutOfStaminaEventHandler eventHandler)
    {
        StartCoroutine(DefeatCoroutine());
    }
    
    private IEnumerator DefeatCoroutine()
    {
        yield return new WaitForSeconds(DefeatDelay);
        PlayerLose();
        
        yield return new WaitForSeconds(CharacterHideDelay);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(ConstTags.Obstacle))
            PlayerLose();
        
        if (collider.CompareTag(ConstTags.WinColider))
            PlayerWin();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(ConstTags.FallingObstacle))
        {
            _stamina.DrainRateStaminaForObstaclesCollision();
            EventAggregator.Post(this, new PopupTextDrainStaminEvent()
            {
                DrainAmount = Mathf.FloorToInt(_characterData.StaminaData.StaminaDrainObstacleCollision)
            });
        }
    }

    #region Gizmoz
        private void OnDrawGizmos() => Gizmos.DrawWireSphere(_transformPlatformDetection.position, 0.2f);
        
    #endregion
}
