using System;
using System.Collections;
using GG.Infrastructure.Utils.Swipe;
using UnityEngine;

public class PlayerController : MonoBehaviour, ICharacterEvents
{
    // можно вынести swiping и вообще весь инпут в инпут класс какой-нибудь 
    
    private const float DefeatDelay = 0.7f;
    private const float CharacterHideDelay = 2f;
    
    public event Action CharacterDefeat; 
    
    [SerializeField] private SwipeListener _swipeListener;
    [SerializeField] private PlayerMover _playerMover;

    private bool _isEnableSwiping = true;
    
    private void OnEnable()
    {
        _swipeListener.OnSwipe.AddListener(OnSwipeHandler);
        _playerMover.RanOutOfStamin += OnRanOutOfStamin;
    }

    
    private void OnDisable()
    {
        _swipeListener.OnSwipe.RemoveListener(OnSwipeHandler);
        _playerMover.RanOutOfStamin -= OnRanOutOfStamin;
    }
    
    private void OnRanOutOfStamin()
    {
        _isEnableSwiping = false;
        StartCoroutine(DefeatCoroutine());
    }

    private void OnSwipeHandler(string swipe)
    {
        if (_isEnableSwiping == false)
            return;

        if (swipe == DirectionId.ID_LEFT)
        {
            _playerMover.Jump(false);
        }

        if (swipe == DirectionId.ID_RIGHT)
        {
            _playerMover.Jump(true);
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
    
    
    private IEnumerator DefeatCoroutine()
    {
        yield return new WaitForSeconds(DefeatDelay);
        CharacterDefeat?.Invoke();
        
        yield return new WaitForSeconds(CharacterHideDelay);
        _playerMover.gameObject.SetActive(false);
    }
}
