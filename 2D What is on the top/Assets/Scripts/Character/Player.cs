using System;
using System.Collections;
using GG.Infrastructure.Utils.Swipe;
using UnityEngine;

public class Player : MonoBehaviour, IPlayer
{
    private const float DefeatDelay = 0.7f;
    private const float CharacterHideDelay = 2f;

    public event Action<bool> PlayerIsOnRightWall;
    public event Action LevelDefeat; 
    public event Action LevelWin;
    
    [SerializeField] private SwipeListener _swipeListener;
    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private TriggerWinLevel _triggerWinLevel;

    private bool _isBlockSpwipe = false;
    private bool _isBlockMovement = false;
    private bool _isPlayerLose = false;
    
    private void OnEnable()
    {
        _swipeListener.OnSwipe.AddListener(OnSwipeHandler);
        _playerMover.RanOutOfStamin += OnRanOutOfStamin;
        _triggerWinLevel.LevelWin += OnPlayerWin;
    }

    private void OnDisable()
    {
        _swipeListener.OnSwipe.RemoveListener(OnSwipeHandler);
        _playerMover.RanOutOfStamin -= OnRanOutOfStamin;
        _triggerWinLevel.LevelWin -= OnPlayerWin;
    }

    private void Update()
    {
        if (_isPlayerLose)
            return;
        
        ResetSlowDownToTouch();
        _playerMover.ProcessCheckingToPlayerAction();
    }

    private void FixedUpdate()
    {
        if (_isPlayerLose)
            return;
        
        _playerMover.ProcessMovement(_isBlockMovement);
    }

    public void BlockSwipe(bool isBlock) => _isBlockSpwipe = isBlock;
    
    public void PlayerLose()
    {
        _isBlockSpwipe = true;
        _isBlockMovement = true;
        _isPlayerLose = true;
        LevelDefeat?.Invoke();
    }

    private void ResetSlowDownToTouch()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            _playerMover.SlowDownFlag(false); 
    }
    
    private void OnSwipeHandler(string swipe)
    {
        if (_isBlockSpwipe)
            return;

        if (swipe == DirectionId.ID_LEFT)
        {
            _playerMover.Jump(false);
            PlayerIsOnRightWall?.Invoke(false);
        }

        if (swipe == DirectionId.ID_RIGHT)
        {
            _playerMover.Jump(true);
            PlayerIsOnRightWall?.Invoke(true);
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
    
    private void OnRanOutOfStamin()
    {
        _isBlockSpwipe = true;
        StartCoroutine(DefeatCoroutine());
    }

    private void OnPlayerWin()
    {
        _isBlockMovement = true;
        _playerMover.FreezePlayer(true);
        LevelWin?.Invoke();
    }
    
    private IEnumerator DefeatCoroutine()
    {
        yield return new WaitForSeconds(DefeatDelay);
        LevelDefeat?.Invoke();
        
        yield return new WaitForSeconds(CharacterHideDelay);
        _playerMover.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(ConstTags.Obstacle))
            PlayerLose();
    }
}
